using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PowerUpItem
{
    public float health;
    public FloatValue maxHealth;
    public string enemyName;
    public int baseDamage;
    public float moveSpeed;
    public EnemyState currentState;
    public GameObject deathEffect;
    public LootTable enemyLoot;

    private void Awake()
    {
        health = maxHealth.initialValue;
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            DeathEffect();
            DropLoot();
            this.gameObject.SetActive(false);
        }
    }

    private void DropLoot()
    {
        if(enemyLoot != null)
        {
            PowerUpItem current = enemyLoot.lootPowup();
            if(current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    private void DeathEffect()
    {
        if(deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    }

    public void Knock(Rigidbody2D enemyRigid, float knockBackTime, float damage)
    {
        StartCoroutine(KnockCouroutine(enemyRigid, knockBackTime));
        TakeDamage(damage);
    }

    private IEnumerator KnockCouroutine(Rigidbody2D enemyRigid, float knockBackTime)
    {
        if (enemyRigid != null)
        {
            yield return new WaitForSeconds(knockBackTime);
            enemyRigid.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            enemyRigid.velocity = Vector2.zero;
        }
    }

}
