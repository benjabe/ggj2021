using UnityEngine;

public class LifeSupportSystem : ShipSystem
{
    [Tooltip("Maps amount of working components (the index of the entry) to O2 delta per second (the value, positive = gain o2, negative = lose o2). Make sure this has as many entries as the amount of components that can be attached plus one! e.g with 3 components there should be 4 entries (0, 1, 2, 3).")]
    [SerializeField] private float[] _workingComponentCountToO2DeltaPerSecond;

    public override void EndAwake()
    {
    }

    public override void EndStart()
    {
    }

    public override void UpdateAccordingToWorkingComponentCount(int count)
    {
        var o2Delta = _workingComponentCountToO2DeltaPerSecond[count] * Time.deltaTime;
        Ship.ChangeStatValue("O2", o2Delta);
    }
}
