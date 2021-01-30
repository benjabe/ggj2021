using UnityEngine;

[CreateAssetMenu(fileName = "Ship System Component Data", menuName = "ScriptableObject/Ship System Component Data", order = 1)]
public class ShipSystemComponentData : ScriptableObject
{
    [SerializeField] private string _name = "Component Name";
    [SerializeField] private Sprite _sprite = null;

    public string Name { get => _name; }
    public Sprite Sprite { get => _sprite; }
}
