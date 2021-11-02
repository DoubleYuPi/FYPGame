using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] float attackForce;
    public float knockBackTime;
    public float damage;
    public FloatValue maxDamage;

    private void Awake()
    {
        damage = maxDamage.initialValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Pot>().Destroy();
        }
        if (collision.gameObject.CompareTag("sphere") && this.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Sphere>().Smashed();
        }
        else if (collision.gameObject.CompareTag("enemy") || collision.CompareTag("Player") && collision.isTrigger)
        {
            if (collision.gameObject.CompareTag("enemy") && gameObject.CompareTag("enemy")) return;

            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * attackForce;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if (collision.gameObject.CompareTag("enemy") && collision.isTrigger)
                {
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    collision.GetComponent<Enemy>().Knock(hit, knockBackTime, damage);
                }
                //StartCoroutine(KnockCouroutine(enemy));
                if (collision.gameObject.CompareTag("Player"))
                {
                    if (collision.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                    {
                        hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                        collision.GetComponent<PlayerMovement>().Knock(knockBackTime, damage);
                    }
                }
            }
        }
    }
}
