using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PauseManager : MonoBehaviour
{
    private bool paused;
    public GameObject pausePanel;
    void Start()
    {
        paused = false;
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }

    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
