using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    void Start()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject timer = GameObject.Find("TimerText");
        timer.GetComponent<Timer>().finish();
        SceneManager.LoadScene("Win");
    }
}
