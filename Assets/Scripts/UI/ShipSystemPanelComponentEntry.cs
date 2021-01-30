using UnityEngine;
using UnityEngine.UI;

public class ShipSystemPanelComponentEntry : MonoBehaviour
{
    [SerializeField] private Image _componentImage = null;
    [SerializeField] private Text _componentNameText = null;
    [SerializeField] private Text _componentConditionText = null;
    [SerializeField] private RectTransform _handle = null;
    [SerializeField] private Button _repairButton = null;

    private bool _hasMoved = false;
    private Canvas _canvas = null;
    private bool _isDragging = false;
    private Vector3 _previousMousePosition;
    private RectTransform _rectTransform = null;
    private ShipSystemPanel _parentPanel = null;

    public ShipSystemComponent Component { get; set; }

    private void Awake()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _parentPanel = transform.parent.GetComponent<ShipSystemPanel>();
        _repairButton.onClick.AddListener(StartComponentRepairJob);
    }

    private void Start()
    {
        _componentImage.sprite = Component.Sprite;
        _componentNameText.text = Component.Name;
        _componentConditionText.text = $"Condition: {Component.Condition}";
        //Debug.Log($"ShipSystemPanelComponentEntry for component {Component.Name} initialised.");
    }

    private void Update()
    {
        _componentConditionText.text = $"Condition: {(int)(Component.Condition * 10) / 10.0f}";
        if (Component.Condition < 100.0f)
        {
            _repairButton.gameObject.SetActive(true);
        }
        else
        {
            _repairButton.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            // Start dragging.
            if (IsMouseInRect(_handle))
            {
                _isDragging = true;
                // And set canvas to parent so we don't get screwed by the layout group.
                transform.SetParent(_canvas.transform);
            }
        }
        if (_isDragging && Input.GetMouseButton(0))
        {
            // Drag.
            var mouseDelta = Input.mousePosition - _previousMousePosition;
            if (_hasMoved)
            {
                _rectTransform.anchoredPosition += new Vector2(mouseDelta.x, mouseDelta.y);
            }
            else
            {
                _hasMoved = true;
            }
        }
        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            // Stop dragging.
            _isDragging = false;
            var otherPanel = GetOtherShipSystemPanelUnderMouse();
            if (otherPanel != null)
            {
                EnqueueComponentMoveJobs(otherPanel);
            }
            transform.SetParent(_parentPanel.transform);
        }
        _previousMousePosition = Input.mousePosition;
    }

    private bool EnqueueComponentMoveJobs(ShipSystemPanel otherPanel)
    {
        // Try to make the system on the other panel the system of the component
        if (otherPanel.ShipSystem.CanAddComponent(Component))
        {
            // Give the astronaut the order to uninstall the component from its old system
            // And install the component to the other system upon completion.
            if (_parentPanel.ShipSystem is Astronaut)
            {
                var installJob = new InstallComponentJob(otherPanel.ShipSystem, Component);
                if (installJob.CheckInstantiationPrerequisite())
                {
                    Job.QueueJob(installJob);
                    Debug.Log($"ShipSystemPanelComponentEntry: Started installing {Component.Name} to {otherPanel.ShipSystem.Name}");
                    //Destroy(gameObject);
                    return true;
                }
            }
            else
            {
                var uninstallJob = new UninstallComponentJob(_parentPanel.ShipSystem, Component);
                if (uninstallJob.CheckInstantiationPrerequisite())
                {
                    if (otherPanel.ShipSystem is Astronaut)
                    {
                        Job.QueueJob(uninstallJob);
                        Debug.Log($"ShipSystemPanelComponentEntry: Started uninstalling {Component.Name} from {_parentPanel.ShipSystem.Name}");
                        return true;
                    }
                    else
                    {
                        var installJob = new InstallComponentJob(otherPanel.ShipSystem, Component);
                        if (installJob.CheckInstantiationPrerequisite())
                        {
                            // Both jobs are possible! Let's queue them both.
                            Job.QueueJob(uninstallJob);
                            Job.QueueJob(installJob);
                            Debug.Log($"ShipSystemPanelComponentEntry: Started moving {Component.Name} to {otherPanel.ShipSystem.Name}");
                            //Destroy(gameObject);
                            return true;
                        }
                    }
                }
            }
        }
        Debug.Log($"ShipSystemPanelComponentEntry: Failed to set {Component.Name}'s parent to {otherPanel.ShipSystem.Name}");
        return false;
    }

    private bool IsMouseInRect(RectTransform rt)
    {
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition, _canvas.worldCamera);
        }
        else
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition);
        }
    }
    private ShipSystemPanel GetOtherShipSystemPanelUnderMouse()
    {
        foreach (var panel in ShipSystemPanel.ShipSystemPanels)
        {
            if (panel == _parentPanel) continue;
            if (IsMouseInRect(panel.RectTransform)) return panel;
        }
        return null;
    }
    private void StartComponentRepairJob()
    {
        Job.QueueJob(new RepairComponentJob(_parentPanel.ShipSystem, Component));
    }
}
