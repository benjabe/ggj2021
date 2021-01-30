using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private static List<ShipStat> _stats = new List<ShipStat>()
    {
        new O2Stat(),
        new AsteroidDeflectionStrengthStat(),
        new HO2Stat()
    };

    private void Update()
    {
        _stats.ForEach(stat => stat.Update());
    }

    /// <summary>
    /// Changes a stat value by a given delta.
    /// </summary>
    /// <param name="name">The stat name.</param>
    /// <param name="delta">The amount by which to change the stat's value. Can be negative or positive.</param>
    public static void ChangeStatValue(string name, float delta)
    {
        var stat = GetStatWithName(name);
        stat.Value = Mathf.Clamp(stat.Value + delta, 0.0f, 100.0f);
    }
    /// <summary>
    /// Changes a stat's value to a given value.
    /// </summary>
    /// <param name="name">The stat name.</param>
    /// <param name="value">The new stat value.</param>
    public static void SetStatValue(string name, float value)
    {
        var stat = GetStatWithName(name);
        stat.Value = Mathf.Clamp(value, 0.0f, 100.0f);
    }

    public static float GetStatValue(string name)
    {
        return GetStatWithName(name).Value;
    }

    public static List<string> GetStatNames()
    {
        return _stats.Select(s => s.Name).ToList();
    }

    public static ShipStat GetStatWithName(string name)
    {
        var stat = _stats.Where(s => s.Name == name).FirstOrDefault();
        if (stat == null)
        {
            Debug.LogError($"No stat with name '{name}'!");
        }
        return stat;
    }
}
