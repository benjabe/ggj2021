using System.Collections.Generic;
using UnityEngine;

public class ShipSystemPanel : MonoBehaviour
{
    public static List<ShipSystemPanel> ShipSystemPanels { get; private set; } = new List<ShipSystemPanel>();

    [SerializeField] private GameObject _shipSystemPanelComponentEntryPrefab = null;

    private List<GameObject> _entries = new List<GameObject>();

    public ShipSystem ShipSystem { get; set; } = null;
    public RectTransform RectTransform { get; private set; } = null;

    private void Awake()
    {
        ShipSystemPanels.Add(this);
        RectTransform = GetComponent<RectTransform>();
    }

    public void Start()
    {
        ShipSystem.OnComponentAdded += OnComponentAdded;
        ShipSystem.OnComponentRemoved += OnComponentRemoved;
        UpdateComponentEntries();
    }

    private void OnDestroy()
    {
        ShipSystem.OnComponentAdded -= OnComponentAdded;
        ShipSystem.OnComponentRemoved -= OnComponentRemoved;
        ShipSystemPanels.Remove(this);
    }

    private void OnComponentAdded(ShipSystemComponent component)
    {
        UpdateComponentEntries();
    }

    private void OnComponentRemoved(ShipSystemComponent component)
    {
        UpdateComponentEntries();
    }

    private void UpdateComponentEntries()
    {
        foreach (var entry in _entries)
        {
            Destroy(entry);
        }
        _entries = new List<GameObject>();
        foreach (var component in ShipSystem.GetSystemComponents())
        {
            var go = Instantiate(_shipSystemPanelComponentEntryPrefab, transform);
            go.GetComponent<ShipSystemPanelComponentEntry>().Component = component;
            _entries.Add(go);
        }
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        //GetComponent<VerticalLayoutGroup>().enabled = false;
        //GetComponent<VerticalLayoutGroup>().enabled = true;
    }
}
