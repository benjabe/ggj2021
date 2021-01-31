using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OxygenBar : MonoBehaviour
{
    public static OxygenBar Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Slider slider;

    public void SetMaxOxygen(float oxygen)
    {
        slider.maxValue = oxygen;
        slider.value = oxygen;
    }
    
    
    public void SetOxygen(float oxygen)
    {
        slider.value = Mathf.Clamp(oxygen, 0f, 100f);
    }

    
}
