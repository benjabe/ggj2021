using UnityEngine;

[CreateAssetMenu(fileName = "Ship System Component Data", menuName = "ScriptableObject/Ship System Component Data", order = 1)]
public class ShipSystemComponentData : ScriptableObject
{
    [SerializeField] private string _name = "Component Name";
    [SerializeField] private Sprite _sprite = null;
    [Tooltip("Mulitplies by the base time to uninstall components and the systems's multiplier.")]
    [Min(0.00001f)]
    [SerializeField] private float _uninstallTimeMultiplier = 1.0f;
    [Tooltip("Mulitplies by the base time to install components and the systems's multiplier.")]
    [Min(0.00001f)]
    [SerializeField] private float _installTimeMultiplier = 1.0f;
    [Tooltip("Mulitplies by the base time to repair components and the system's multiplier.")]
    [Min(0.00001f)]
    [SerializeField] private float _repairTimeMultiplier = 1.0f;

    public string Name { get => _name; }
    public Sprite Sprite { get => _sprite; }
    public float UninstallTimeMultiplier { get => _uninstallTimeMultiplier; }
    public float InstallTimeMultiplier { get => _installTimeMultiplier; }
    public float RepairTimeMultiplier { get => _repairTimeMultiplier; }

}
