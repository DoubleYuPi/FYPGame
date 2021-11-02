using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PowerUpItem
{
    public FloatValue playerHealth;
    public float healHealth;
    public FloatValue heartContainers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            playerHealth.runTimeVal += healHealth;
            if(playerHealth.initialValue > heartContainers.runTimeVal * 2f)
            {
                playerHealth.initialValue = heartContainers.runTimeVal * 2f;
            }
            powerupAlerts.Raise();
            Destroy(this.gameObject);
        }
    }
}
