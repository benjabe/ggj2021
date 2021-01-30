using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ShipSystem : MonoBehaviour
{
    [Tooltip("Set this to Prefabs/UI/ShipSystemPanel")]
    [SerializeField] private GameObject _shipSystemPanel = null;
    [SerializeField] private Transform _shipSystemPanelPositionTransform = null;
    [SerializeField] private string _name = "System Name";
    [Tooltip("The system's compatible components. The system starts with one of each, and is fully operational with all these components.")]
    [SerializeField] private ShipSystemComponentData[] _compatibleComponents = null;

    private List<ShipSystemComponent> _components = null;
    private GameObject _panel = null;

    public string Name { get => _name; }
    public Action<ShipSystemComponent> OnComponentAdded { get; set; } = null;
    public Action<ShipSystemComponent> OnComponentRemoved { get; set; } = null;

    public abstract void UpdateAccordingToWorkingComponentCount(int count);

    private void Awake()
    {
        _components = _compatibleComponents.ToList().Select(c => new ShipSystemComponent(c)).ToList();
    }

    private void Update()
    {
        UpdateAccordingToWorkingComponentCount(GetWorkingComponentCount());

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_components.Count > 0)
            {
                RemoveSystemComponent(_components[0]);
            }
        }
    }

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
        if (HasComponentOfType(component.Data))
        {
            // We can add this
            Debug.Log($"Component {component.Name} fits {Name}");
            if (!_components.Contains(component))
            {
                _components.Add(component);
                Debug.Log($"{component.Name} was added to {Name}!");
                OnComponentAdded?.Invoke(component);
            }
            else
            {
                Debug.Log($"However, {Name} already has a {component.Name}. Put it somewehere else!");
            }
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
}
