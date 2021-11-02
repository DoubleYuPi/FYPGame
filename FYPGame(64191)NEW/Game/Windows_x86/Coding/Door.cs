using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObj
{
    [Header("Door variables")]
    public bool open = false;
    public DoorType doorType;

    public SpriteRenderer doorSprite;
    public BoxCollider2D physicalCollider;

    // Update is called once per frame
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            if (triggerPlayer)
            {

            }
        }*/
    }
    public void OpenDoor()
    {
        doorSprite.enabled = false;
        open = true;
        physicalCollider.enabled = false;
    }

    public void CloseDoor()
    {

    }

    
}
