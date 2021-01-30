using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storyteller : MonoBehaviour
{
    [Header("General Parameters")]
    [Tooltip("The minimum time between events (like asteroid impact, system damage, etc.) in seconds.")]
    [SerializeField, Min(0)] private float _minTimeBetweenEvents = 10.0f;
    [Tooltip("The maximum time between events (like asteroid impact, system damage, etc.) in seconds.")]
    [SerializeField, Min(0)] private float _maxTimeBetweenEvents = 20.0f;
    [Header("Random Ship System Damage")]
    [Tooltip("The minimum damage that can be done to a system when a random ship system is damaged.")]
    [SerializeField, Min(0)] private float _minShipSystemDamage = 10.0f;
    [Tooltip("The maximum damage that can be done to a system when a random ship system is damaged.")]
    [SerializeField, Min(0)] private float _maxShipSystemDamage = 100.0f;

    private float _nextEventTime;
    private List<ShipSystem> shipSystems;

    private void Awake()
    {
        SetNextEventTime();
        shipSystems = FindObjectsOfType<ShipSystem>().ToList();
    }

    private void Update()
    {
        if (Time.time >= _nextEventTime)
        {
            TriggerEvent();
        }
    }

    private void SetNextEventTime()
    {
        _nextEventTime = Time.time + Random.Range(_minTimeBetweenEvents, _maxTimeBetweenEvents);
        Debug.Log($"Storyteller: Next event set to occur at {_nextEventTime} seconds.");
    }

    /// <summary>
    /// Triggers a random event.
    /// </summary>
    private void TriggerEvent()
    {
        Debug.Log("Storyteller: Random event triggered!");
        DamageRandomShipSystem();
        SetNextEventTime();
    }

    // ---- Create new events down here ----

    /// <summary>
    /// Damages a random ship system with working components. Fails if no viable systesms where found.
    /// </summary>
    /// <returns></returns>
    private bool DamageRandomShipSystem()
    {
        var systems = shipSystems.Where(s => s.IsRandomDamageTarget && s.GetWorkingComponentCount() > 0);
        if (systems.Count() > 0)
        {
            var system = systems.Random();
            Debug.Log($"Storyteller: Damage to random ship system ({system.Name}).");
            system.DamageComponents(Random.Range(_minShipSystemDamage, _maxShipSystemDamage));
            return true;
        }
        else
        {
            Debug.Log($"Storyteller: Failed to find a ship system which is a damage target and has working components.");
            return false;
        }
    }
}
