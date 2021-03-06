﻿using UnityEngine;

public class H20Stat : ShipStat
{
    public override string Name { get => "H2O"; }

    public override void Update()
    {
        if (Value <= 0.0f)
        {
            // Lose game if water reaches 0.
            Debug.Log($"H2O reached 0, game lost!");
        }
    }
    public override void Initialize()
    {
        OnValueChanged += WaterBar.Instance.SetWater;
    }
}
