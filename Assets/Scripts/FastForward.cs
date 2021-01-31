using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FastForward : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private float[] speed;
    public void Awake()
    {
        for (var i = 0; i < buttons.Length; i++)
        {
            var j = i;
            buttons[i].onClick.AddListener(() => { Time.timeScale = speed[j]; Debug.Log("Speed To " + j + ": " + speed[j]); });
            Debug.Log($"Initialized button {i}");
        }
    }
}
