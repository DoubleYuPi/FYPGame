using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEnemy : Slime
{
    public GameObject projectile;
    public float fireRate;
    private float fireRateSec;
    public bool canCast = true;

    private void Update()
    {
        fireRateSec -= Time.deltaTime;
        if (fireRateSec <=0)
        {
            canCast = true;
            fireRateSec = fireRate;
        }
    }

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= moveRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                if (canCast)
                {
                    Vector3 tempVector = target.transform.position - transform.position;
                    GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                    current.GetComponent<Projectile>().Cast(tempVector);
                    canCast = false;
                    ChangeState(EnemyState.walk);
                    animator.SetBool("move", true);
                }
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > moveRadius)
        {
            animator.SetBool("move", false);
        }
    }
}
