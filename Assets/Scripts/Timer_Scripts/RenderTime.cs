using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderTime : MonoBehaviour
{
    void Start()
    {
        string time = LocalStorage.time;
        Text text = this.transform.GetComponent<Text>();
        text.text = time;
    }

    void Update()
    {
        
    }
}
