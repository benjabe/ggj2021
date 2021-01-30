using UnityEngine;

public class O2Stat : ShipStat
{
    public override string Name { get => "O2"; }

    public override void Update()
    {
        if (Value <= 0.0f)
        {
            // Lose game if oxygen reaches 0.
            Debug.Log($"O2 reached 0, game lost!");
        }
    }
}
