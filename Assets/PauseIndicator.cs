using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseIndicator : MonoBehaviour
{
    private Text _text;    

    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            _text.text = "Paused.";
        }
        else
        {
            _text.text = $"Playing at {Time.timeScale}x speed.";
        }
    }
}
