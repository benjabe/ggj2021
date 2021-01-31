using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaterBar : MonoBehaviour
{
    public static WaterBar Instance;
    public Slider slider;

    public void Awake()
    {
        Instance = this;
    }

    public void SetMaxWater(float water)
    {
        slider.maxValue = water;
        slider.value = water;
    }
    
    
    public void SetWater(float water)
    {
        slider.value = Mathf.Clamp(water, 0 , 100);
    }

    
    
    
    
    
    
}
