using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaterBar : MonoBehaviour
{
    
    public Slider slider;

    public void SetMaxWater(int water)
    {
        slider.maxValue = water;
        slider.value = water;
    }
    
    
    public void SetWater(int water)
    {
        slider.value = water;
    }

    
    
    
    
    
    
}
