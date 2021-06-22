using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pauza : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void Quit() {
        Application.Quit();
    }
}
