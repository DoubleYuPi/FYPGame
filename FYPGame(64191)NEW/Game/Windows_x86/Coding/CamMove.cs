using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Transform target;
    public float smooth;
    public Vector2 maxPos;
    public Vector2 minPos;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPos.x, maxPos.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPos.y, maxPos.y);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smooth);
        }
    }

    public void ScreenKick()
    {
        animator.SetBool("playerHit", true);
        StartCoroutine(kickCoroutine());
    }

    public IEnumerator kickCoroutine()
    {
        yield return null;
        animator.SetBool("playerHit", false);
    }
}
