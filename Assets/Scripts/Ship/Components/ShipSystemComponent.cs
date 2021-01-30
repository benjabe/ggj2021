using UnityEngine;

public class ShipSystemComponent
{
    public float Condition { get; private set; } = 100.0f;
    public Mode CurrentMode { get; private set; }

    public ShipSystemComponentData Data { get; private set; }
    public string Name { get => Data.Name; }
    public Sprite Sprite { get => Data.Sprite; }
    public float UninstallTimeMultiplier { get => Data.UninstallTimeMultiplier; }
    public float InstallTimeMultiplier { get => Data.InstallTimeMultiplier; }
    public float RepairTimeMultiplier { get => Data.RepairTimeMultiplier; }

    public ShipSystemComponent(ShipSystemComponentData data)
    {
        Data = data;
    }

    /// <summary>
    /// Damages component
    /// </summary>
    /// <param name="damageAmt">How much to damage by</param>
    /// <returns>True if condition reaches 0%</returns>
    public bool Damage(float damageAmt)
    {
        Condition -= damageAmt;
        if (Condition <= 0.0f)
        {
            // Broken!
            CurrentMode = Mode.Broken;
            Condition = 0.0f;
            Debug.Log($"{Name} broke!");
        }
        return Condition == 0.0f;
    }
    /// <summary>
    /// Repairs component
    /// </summary>
    /// <param name="repairAmt">How much to repair by</param>
    /// <returns>True if condition reaches 100%</returns>
    public bool Repair(float repairAmt)
    {
        Condition += repairAmt;
        if (CurrentMode == Mode.Broken && Condition > 1.0f)
        {
            CurrentMode = Mode.Working;
            Debug.Log($"{Name} fixed!");
        }
        if (Condition > 100.0f)
        {
            Condition = 100.0f;
        }
        return Condition == 100.0f;
    }
    public enum Mode
    {
        Working,
        Broken
    }
}
