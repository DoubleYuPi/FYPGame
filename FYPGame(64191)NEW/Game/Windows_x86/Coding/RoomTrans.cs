using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrans : MonoBehaviour
{
    public Vector2 cameraMove;
    public Vector3 playerMove;
    private CamMove cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CamMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            cam.minPos += cameraMove;
            cam.maxPos += cameraMove;
            collision.transform.position += playerMove;
        }
    }
}
