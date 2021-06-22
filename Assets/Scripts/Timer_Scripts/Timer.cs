using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text timerText;
    private float startTime;
    bool finished = false;

    void Start()
    {
        //DontDestroyOnLoad(transform.gameObject);
        startTime = Time.time;
    }

    void Update()
    {
        if (!finished)
        {
            float t = Time.time - startTime;

            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            timerText.text = minutes + ":" + seconds;
        }
    }

    public void finish()
    {
        string time = timerText.text;
        LocalStorage.time = time;
        finished = true;
        timerText.color = Color.red;
    }
}
