using UnityEngine;

public class AsteroidDeflectionSystem : ShipSystem
{
    [Tooltip("Maps amount of working components (the index of the entry) to asteroid deflection strength. Make sure this has as many entries as the amount of components that can be attached plus one! e.g with 3 components there should be 4 entries (0, 1, 2, 3).")]
    [SerializeField] private float[] _workingComponentCountToDeflectionStrength;

    public override void EndAwake()
    {
    }

    public override void EndStart()
    {
    }

    public override void UpdateAccordingToWorkingComponentCount(int count)
    {
        var deflectionStrength = _workingComponentCountToDeflectionStrength[count];
        Ship.SetStatValue("Asteroid Deflection Strength", deflectionStrength);
    }
}
