using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : SingletonMonoBehaviour<PlayerMovement>
{
    private WaitForSeconds afterUseToolsAnimationPause;
    private WaitForSeconds useToolsAnimationPause;
    private bool playerToolUseDisabled = false;
    private AnimationOverride animationOverride;
    public float speed;
    private Rigidbody2D mcRigid;
    private Vector3 pos;
    private Animator animator;
    public PlayerState currentState;
    public FloatValue currentHealth;
    public Alerts playerHeartAlerts;
    public Alerts playerHit;
    public GameObject fireBallProjectile;
    public Alerts useMagic;
    public SpellContainer playerMagic;
    private ToolEffect toolEffect = ToolEffect.none;

    private List<CharacterAttribute> characterAttributesList;
    [SerializeField] private SpriteRenderer HoldItem = null;

    private CharacterAttribute statePlayer;
    private CharacterAttribute toolPlayer;

    private GridCursor gridCursor;

    private bool _playerInputIsDisabled = false;
    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        mcRigid = GetComponent<Rigidbody2D>();

        animationOverride = GetComponentInChildren<AnimationOverride>();
        statePlayer = new CharacterAttribute(PlayerState.picking,PartVariantType.none);
        characterAttributesList = new List<CharacterAttribute>();

        gridCursor = FindObjectOfType<GridCursor>();

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentState == PlayerState.interact)
        {
            return;
        }
        pos = Vector3.zero;
        pos.x = Input.GetAxisRaw("Horizontal");
        pos.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Attacking") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            SoundManager.PlaySound("swing");
            StartCoroutine(Attack());
        }else if (Input.GetButtonDown("Spell") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            SoundManager.PlaySound("fireball1");
            StartCoroutine(Spell());
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            MoveAnimation();
        }
        TestInput();
        PlayerClickInput();
    }

    private IEnumerator Attack()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if(currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private IEnumerator Spell()
    {
        currentState = PlayerState.spell;
        yield return null;
        makeFire();
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private void makeFire()
    {
        if (playerMagic.currentMana > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            FireBall fireBall = Instantiate(fireBallProjectile, transform.position, Quaternion.identity).GetComponent<FireBall>();
            fireBall.Setup(temp, FireBallDirection());
            useMagic.Raise();
        }
    }

    Vector3 FireBallDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveX"), animator.GetFloat("moveY")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    void MoveAnimation()
    {
        if (pos != Vector3.zero)
        {
            Movement();
            animator.SetFloat("moveX", pos.x);
            animator.SetFloat("moveY", pos.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    private IEnumerator KnockCouroutine(float knockBackTime)
    {
        playerHit.Raise();
        if (mcRigid != null)
        {
            yield return new WaitForSeconds(knockBackTime);
            mcRigid.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            mcRigid.velocity = Vector2.zero;
        }
    }

    public void Knock(float knockBackTime, float damage)
    {
        currentHealth.runTimeVal -= damage;
        playerHeartAlerts.Raise();
        if (currentHealth.runTimeVal > 0)
        {
            SoundManager.PlaySound("ouch");
            StartCoroutine(KnockCouroutine(knockBackTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    void Movement()
    {
        mcRigid.MovePosition(transform.position + pos.normalized * speed * Time.deltaTime);
    }

    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if(itemDetails != null)
        {
            HoldItem.sprite = itemDetails.itemIcon;
            HoldItem.color = new Color(1f, 1f, 1f, 1f);

            statePlayer.partVariantType = PartVariantType.carry;
            characterAttributesList.Clear();
            characterAttributesList.Add(statePlayer);
            animationOverride.ApplyCharaCustomParam(characterAttributesList);

            animator.SetBool("carrying", true);
        }
    }

    public void ClearCarriedItem()
    {
        HoldItem.sprite = null;
        HoldItem.color = new Color(0f, 0f, 0f, 0f);

        statePlayer.partVariantType = PartVariantType.none;
        characterAttributesList.Clear();
        characterAttributesList.Add(statePlayer);
        animationOverride.ApplyCharaCustomParam(characterAttributesList);

        animator.SetBool("carrying", false);
    }

    private void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneControlManager.Instance.FadeAndLoadScene(Places.Overworld.ToString(), transform.position);
        }
    }

    private void PlayerClickInput()
    {
        if (!playerToolUseDisabled)
        {
            if (Input.GetMouseButton(0))
            {
                if (gridCursor.CursorIsEnabled)
                {
                    Vector3Int cursorGridPos = gridCursor.GetGridPositionForCursor();
                    Vector3Int playerGridPos = gridCursor.GetGridPositionForPlayer();
                    ProcessPlayerClickInput(cursorGridPos, playerGridPos);
                }
            }
        }
    }

    private void ProcessPlayerClickInput(Vector3Int cursorGridPos, Vector3Int playerGridPos)
    {
        Vector3Int playerDirection = GetPlayerClickDirection(cursorGridPos, playerGridPos);

        GridPropertiesDetails gridPropertiesDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPos.x, cursorGridPos.y);
        
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if(itemDetails != null)
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ProcessPlayerClickInputSeed(gridPropertiesDetails, itemDetails);
                    }
                    break;
                case ItemType.Fruit:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ProcessPlayerClickInputFruit(itemDetails);
                    }
                    break;
                case ItemType.Watering_Tool:
                case ItemType.Hoeing_Tool:
                    ProcessPlayerClickInputTool(itemDetails,gridPropertiesDetails,playerDirection);
                    break;
                case ItemType.none:
                    break;
                case ItemType.count:
                    break;
            }
        }
    }

    private void ProcessPlayerClickInputTool(ItemDetails itemDetails, GridPropertiesDetails gridPropertiesDetails, Vector3Int playerDirection)
    {
        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_Tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    HoeGround(gridPropertiesDetails, playerDirection);
                }
                break;
            case ItemType.Watering_Tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    WaterGround(gridPropertiesDetails,playerDirection);
                }
                break;

            default:
                break;
        }
    }

    private void HoeGround(GridPropertiesDetails gridPropertiesDetails, Vector3Int playerDirection)
    {
        StartCoroutine(HoeGroundRoutine(playerDirection, gridPropertiesDetails));
    }

    private IEnumerator HoeGroundRoutine(Vector3Int playerDirection, GridPropertiesDetails gridPropertiesDetails)
    {
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        toolPlayer.partVariantType = PartVariantType.hoe;
        characterAttributesList.Clear();
        characterAttributesList.Add(toolPlayer);

        yield return useToolsAnimationPause;

        // Set Grid property details for dug ground
        if (gridPropertiesDetails.timeDug == -1)
        {
            gridPropertiesDetails.timeDug = 0;
        }

        // Set grid property to dug
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertiesDetails.gridX, gridPropertiesDetails.gridY, gridPropertiesDetails);

        // Display dug grid tiles
        GridPropertiesManager.Instance.DisplayDugGround(gridPropertiesDetails);

        yield return afterUseToolsAnimationPause;

        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    private void WaterGround(GridPropertiesDetails gridPropertiesDetails, Vector3Int playerDirection)
    {
        StartCoroutine(WaterGroundRoutine(playerDirection, gridPropertiesDetails));
    }

    private IEnumerator WaterGroundRoutine(Vector3Int playerDirection, GridPropertiesDetails gridPropertiesDetails)
    {
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        toolPlayer.partVariantType = PartVariantType.wateringCan;
        characterAttributesList.Clear();
        characterAttributesList.Add(toolPlayer);
        
        toolEffect = ToolEffect.watering;
        yield return useToolsAnimationPause;

        // Set Grid property details for dug ground
        if (gridPropertiesDetails.timeWatered == -1)
        {
            gridPropertiesDetails.timeWatered = 0;
        }

        // Set grid property to dug
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertiesDetails.gridX, gridPropertiesDetails.gridY, gridPropertiesDetails);

        // Display watered grid tiles
        GridPropertiesManager.Instance.DisplayWaterGround(gridPropertiesDetails);

        yield return afterUseToolsAnimationPause;

        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPos, Vector3Int playerGridPos)
    {
        if(cursorGridPos.x > playerGridPos.x)
        {
            return Vector3Int.right;
        }
        else if(cursorGridPos.x < playerGridPos.x)
        {
            return Vector3Int.left;
        }
        else if(cursorGridPos.y > playerGridPos.y)
        {
            return Vector3Int.up;
        }
        else
        {
            return Vector3Int.down;
        }
    }

    private void ProcessPlayerClickInputSeed(GridPropertiesDetails gridPropertiesDetails,ItemDetails itemDetails)
    {
        if (itemDetails.dropped && gridCursor.CursorPositionIsValid && gridPropertiesDetails.timeDug > -1 && gridPropertiesDetails.seedItemCode == -1)
        {
            PlantSeed(gridPropertiesDetails, itemDetails);
        }
        else if (itemDetails.dropped && gridCursor.CursorPositionIsValid)
        {
            EventHandle.CallDropSelectedItemEvent();
        }
    }

    private void PlantSeed(GridPropertiesDetails gridPropertiesDetails, ItemDetails itemDetails)
    {
        gridPropertiesDetails.seedItemCode = itemDetails.itemCode;
        gridPropertiesDetails.growthTime = 0;

        GridPropertiesManager.Instance.DisplayPlantedCrop(gridPropertiesDetails);
        EventHandle.CallRemoveSelectedItemFromInventory();
    }

    private void ProcessPlayerClickInputFruit(ItemDetails itemDetails)
    {
        if (itemDetails.dropped && gridCursor.CursorPositionIsValid)
        {
            EventHandle.CallDropSelectedItemEvent();
        }
    }
}
