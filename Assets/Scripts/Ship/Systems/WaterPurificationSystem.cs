using UnityEngine;

public class WaterPurificationSystem : ShipSystem
{
    [Tooltip("Maps amount of working components (the index of the entry) to H2O gain per second. Positive = gain water, negative = lose water. Make sure this has as many entries as the amount of components that can be attached plus one! e.g with 3 components there should be 4 entries (0, 1, 2, 3).")]
    [SerializeField] private float[] _workingComponentCountToH2OGainPerSecond;

    public override void EndAwake()
    {
    }

    public override void EndStart()
    {
    }

    public override void UpdateAccordingToWorkingComponentCount(int count)
    {
        var h2oGain = _workingComponentCountToH2OGainPerSecond[count];
        Ship.ChangeStatValue("H2O", h2oGain);
    }
}
