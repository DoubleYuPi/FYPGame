using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private Animator animator;
    public LootTable objLoot;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void DropLoot()
    {
        if (objLoot != null)
        {
            PowerUpItem current = objLoot.lootPowup();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    public void Destroy()
    {
        animator.SetBool("destroy", true);
        SoundManager.PlaySound("potsmashed");
        StartCoroutine(BreakCoroutine());
        DropLoot();
    }

    IEnumerator BreakCoroutine()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    }
}
