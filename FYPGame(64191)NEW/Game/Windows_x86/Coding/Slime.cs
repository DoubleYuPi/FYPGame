using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public Rigidbody2D enemyrigidbody;
    public Transform target;
    public float moveRadius;
    public float attackRadius;
    public Transform statePosition;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        enemyrigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        animator.SetBool("move", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }

    public virtual void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= moveRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                changeAnim(temp - transform.position);
                enemyrigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                animator.SetBool("move", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > moveRadius)
        {
            animator.SetBool("move", false);
        }
    }


    public void changeAnim(Vector2 direction)
    {
        direction = direction.normalized;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
