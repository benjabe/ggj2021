using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class ShipSystem : MonoBehaviour
{
    [Tooltip("Set this to Prefabs/UI/ShipSystemPanel")]
    [SerializeField] private GameObject _shipSystemPanel = null;
    [SerializeField] private Transform _shipSystemPanelPositionTransform = null;
    [SerializeField] private string _name = "System Name";
    [Tooltip("The system's compatible components. The system starts with one of each, and is fully operational with all these components.")]
    [SerializeField] private ShipSystemComponentData[] _compatibleComponents = null;
    [Tooltip("If true, the compatible components list only acts as a starting components list, and all components can be added multiple times.")]
    [SerializeField] private bool _hasUnlimitedComponentCapacity = false;
    [Tooltip("Whether this system can randomly have its components damaged.")]
    [SerializeField] private bool _isRandomDamageTarget = true;
    [SerializeField] private Vector3 _workPosition = Vector3.zero;
    [Tooltip("Mulitplies by the base time to uninstall components and the component's multiplier.")]
    [Min(0.00001f)]
    [SerializeField] private float _uninstallTimeMultiplier = 1.0f;
    [Tooltip("Mulitplies by the base time to install components and the component's multiplier.")]
    [Min(0.00001f)]
    [SerializeField] private float _installTimeMultiplier = 1.0f;
    [Tooltip("Mulitplies by the base time to repair components and the component's multiplier.")]
    [Min(0.00001f)]
    [SerializeField] private float _repairTimeMultiplier = 1.0f;

    private List<ShipSystemComponent> _components = null;
    private GameObject _panel = null;

    public string Name { get => _name; }
    public Vector3 WorkPosition
    {
        get => _workPosition;
        protected set
        {
            _workPosition = value;
            OnWorkPositionChanged?.Invoke(_workPosition);
        }
    }
    public Action<ShipSystemComponent> OnComponentAdded { get; set; } = null;
    public Action<ShipSystemComponent> OnComponentRemoved { get; set; } = null;
    public Action<Vector3> OnWorkPositionChanged { get; set; } = null;
    public float UninstallTimeMultiplier { get => _uninstallTimeMultiplier; }
    public float InstallTimeMultiplier { get => _installTimeMultiplier; }
    public float RepairTimeMultiplier { get => _repairTimeMultiplier; }
    public bool IsRandomDamageTarget { get => _isRandomDamageTarget; }

    public abstract void UpdateAccordingToWorkingComponentCount(int count);

    private void Awake()
    {
        _components = _compatibleComponents.ToList().Select(c => new ShipSystemComponent(c)).ToList();
        EndAwake();
    }

    private void Start()
    {
        EndStart();
    }

    private void Update()
    {
        UpdateAccordingToWorkingComponentCount(GetWorkingComponentCount());
    }

    /// <summary>
    /// Called at the end of base ShipSystem Awake.
    /// </summary>
    public abstract void EndAwake();
    /// <summary>
    /// Called at the end of base ShipSystem Start.
    /// </summary>
    public abstract void EndStart();

    /// <summary>
    /// Checks if the system has all required components.
    /// </summary>
    /// <returns>True if it do have all required components, yes.</returns>
    public bool HasAllRequiredComponents()
    {
        foreach (var requiredComponent in _compatibleComponents)
        {
            if (!HasComponentOfType(requiredComponent))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Returns amount of functional (i.e. not broken) components.
    /// </summary>
    /// <returns>Amount of functional (i.e. not broken) components.</returns>
    public int GetWorkingComponentCount()
    {
        return _components.Where(c => c.CurrentMode == ShipSystemComponent.Mode.Working).Count();
    }

    /// <summary>
    /// Gets how many components can be on this system.
    /// </summary>
    /// <returns>Gets how many components can be on this system.</returns>
    public int GetMaxComponentAmount()
    {
        return _compatibleComponents.Count();
    }

    public void AddSystemComponent(ShipSystemComponent component)
    {
        if (CanAddComponent(component))
        {
            _components.Add(component);
            Debug.Log($"{component.Name} was added to {Name}!");
            OnComponentAdded?.Invoke(component);
        }
        else
        {
            // This doesn't fit the system.
            Debug.LogWarning($"Component {component.Name} doesn't fit {Name}!");
        }
    }

    public void RemoveSystemComponent(ShipSystemComponent component)
    {
        if (_components.Contains(component))
        {
            _components.Remove(component);
            Debug.Log($"Removed {component.Name} from {Name}.");
            OnComponentRemoved?.Invoke(component);
        }
        else
        {
            Debug.LogWarning($"Tried to remove non-existing component {component.Name} from {Name}!");
        }
    }

    public List<ShipSystemComponent> GetSystemComponents()
    {
        return _components;
    }

    public void DamageComponents(float damage)
    {
        // Distribute damage amongst components
        float remainingDamage = damage;
        var log = $"{Name}: Distributed {damage} damage amongst components:";
        while (remainingDamage > 0.0f)
        {
            float minDamage = Mathf.Min(1.0f, remainingDamage);
            float maxDamage = Mathf.Min(50.0f, remainingDamage);
            float damageToComponent = Random.Range(minDamage, maxDamage);
            var component = _components.Random();
            component.Damage(damageToComponent);
            remainingDamage -= damageToComponent;
            log += $"\n{damageToComponent} dealt to {component.Name}.";
        }
        Debug.Log(log);
    }

    private void OnMouseDown()
    {
        Debug.Log($"{Name} clicked!");
        if (_panel == null)
        {
            InstantiatePanel();
        }
        else
        {
            Destroy(_panel);
        }
    }

    private void InstantiatePanel()
    {
        var go = Instantiate(
            _shipSystemPanel,
            _shipSystemPanelPositionTransform.position,
            Quaternion.identity,
            GameObject.Find("Canvas").transform
        );
        go.GetComponent<ShipSystemPanel>().ShipSystem = this;
        _panel = go;
    }

    public bool HasComponentOfType(ShipSystemComponentData data)
    {
        return _components.Where(c => c.Data == data).Any();
    }

    public bool HasComponent(ShipSystemComponent component)
    {
        return _components.Contains(component);
    }

    public bool CanAddComponent(ShipSystemComponent component)
    {
        return _hasUnlimitedComponentCapacity ||
               !HasComponentOfType(component.Data) && _compatibleComponents.Contains(component.Data);
    }

    public List<ShipSystemComponentData> GetRequiredComponents()
    {
        return _compatibleComponents.ToList();
    }
}
