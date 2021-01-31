using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSystemPanel : MonoBehaviour
{
    public static List<ShipSystemPanel> ShipSystemPanels { get; private set; } = new List<ShipSystemPanel>();

    [SerializeField] private GameObject _shipSystemPanelComponentEntryPrefab = null;
    [SerializeField] private Text _systemNameText = null;
    [SerializeField] private Text _requiredComponentsText = null;
    [SerializeField] private Button _closeButton = null;

    private List<GameObject> _entries = new List<GameObject>();

    public ShipSystem ShipSystem { get; set; } = null;
    public RectTransform RectTransform { get; private set; } = null;

    private void Awake()
    {
        ShipSystemPanels.Add(this);
        RectTransform = GetComponent<RectTransform>();
        _closeButton.onClick.AddListener(() =>
        {
            Destroy(gameObject);
            SoundManager.PlaySound("DisengageSystem");
        });
    }

    public void Start()
    {
        _systemNameText.text = ShipSystem.Name;
        var reqCompText = "Required Components:";
        var reqComponents = ShipSystem.GetRequiredComponents();
        foreach (var component in reqComponents)
        {
            reqCompText += "\n- " + component.Name;
        }
        if (reqComponents.Count > 0)
        {
            _requiredComponentsText.text = reqCompText;
        }
        else
        {
            Destroy(_requiredComponentsText);
        }
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
