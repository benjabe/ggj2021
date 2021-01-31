using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storyteller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _eventPanelPrefab = null;
    [Header("General Parameters")]
    [Tooltip("If true there's a random event when the game starts.")]
    [SerializeField] private bool _eventAtStart = true;
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
        shipSystems = FindObjectsOfType<ShipSystem>().ToList();
    }

    private void Start()
    {
        if (!_eventAtStart)
        {
            SetNextEventTime();
        }
        else
        {
            TriggerEvent();
        }
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
            InstantiatePanel($"{system.Name} damaged!", $"Several components were damaged when {system.Name} got damaged. You should fix that.");
            return true;
        }
        else
        {
            Debug.Log($"Storyteller: Failed to find a ship system which is a damage target and has working components.");
            return false;
        }
    }

    private void InstantiatePanel(string eventName, string eventDescription)
    {
        var canvasTransform = GameObject.Find("Canvas").transform;
        var go = Instantiate(_eventPanelPrefab, canvasTransform);
        var panel = go.GetComponent<EventPanel>();
        panel.SetText(eventName, eventDescription);
    }
}
