using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingSlime : Slime
{
    public Transform[] path;
    public int currentPosition;
    public Transform currentGoal;
    public float roundingDistance;

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= moveRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                changeAnim(temp - transform.position);
                enemyrigidbody.MovePosition(temp);
                //ChangeState(EnemyState.walk);
                animator.SetBool("move", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > moveRadius)
        {
            if (Vector3.Distance(transform.position, path[currentPosition].position) > roundingDistance)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPosition].position, moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                enemyrigidbody.MovePosition(temp);
            }
            else
            {
                changeGoal();
                //animator.SetBool("move", false);
            }
        }
    }

    private void changeGoal()
    {
        if(currentPosition == path.Length - 1)
        {
            currentPosition = 0;
            currentGoal = path[0];
        }
        else
        {
            currentPosition++;
            currentGoal = path[currentPosition];
        }
    }
}
