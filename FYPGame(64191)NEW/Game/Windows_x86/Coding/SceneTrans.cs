using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrans : MonoBehaviour
{
    [SerializeField] private Places sceneName = Places.Overworld;
    [SerializeField] private Vector3 goToScene = new Vector3();

    public void OnTriggerStay2D(Collider2D collider)
    {
        PlayerMovement player = collider.GetComponent<PlayerMovement>();

        if (player != null)
        {
            float xPos = Mathf.Approximately(goToScene.x, 0f) ? player.transform.position.x : goToScene.x;
            float yPos = Mathf.Approximately(goToScene.y, 0f) ? player.transform.position.y : goToScene.y;
            float zPos = 0f;

            SceneControlManager.Instance.FadeAndLoadScene(sceneName.ToString(), new Vector3(xPos, yPos, zPos));
        }
    }
}
