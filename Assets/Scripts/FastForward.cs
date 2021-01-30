using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastForward : MonoBehaviour{
    [SerializeField] private Button[] buttons;
    [SerializeField] private float[] speed;
    public void Start(){
        for(var i = 0; i<buttons.Length; i++){
            var j = i;
            buttons[i].onClick.AddListener(() => Time.timeScale = speed[j]);
        }
    }
}
