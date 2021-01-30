public abstract class ShipStat
{
    public abstract string Name { get; }
    public float Value { get; set; } = 100.0f;

    public abstract void Update();
}
