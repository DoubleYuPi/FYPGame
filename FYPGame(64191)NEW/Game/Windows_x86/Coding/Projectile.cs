using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int speed;
    public Vector2 directionMove;
    public float lifetime;
    private float lifetimeSec;
    public Rigidbody2D ballRigid;

    public void Awake()
    {
        ballRigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lifetimeSec = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeSec -= Time.deltaTime;
        if (lifetimeSec <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Cast(Vector2 initialVelocity)
    {
        ballRigid.velocity = initialVelocity * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
