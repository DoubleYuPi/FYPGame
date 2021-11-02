using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;
    public Rigidbody2D fireBall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Setup(Vector2 velocity, Vector3 direction)
    {
        fireBall.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction); //quaternion find the direction in 3d space  
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
