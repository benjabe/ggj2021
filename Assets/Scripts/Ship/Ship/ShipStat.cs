using System;

public abstract class ShipStat
{
    public abstract string Name { get; }
    public Action<float> OnValueChanged;
    public float Value { get => _value; set { _value = value; OnValueChanged?.Invoke(value); } }
    private float _value = 100.0f;

    public abstract void Update();
    public abstract void Initialize();
}
