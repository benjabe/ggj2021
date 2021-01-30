using UnityEngine;
using UnityEngine.UI;

public class MissionCard : MonoBehaviour
{
    public MissionEvent MissionEvent { get; set; }

    private void Start()
    {
        var text = $"{MissionEvent.Name} ({TimeSystem.FormatAsDate(MissionEvent.TimeOfEvent)})";
        foreach (var condition in MissionEvent.Conditions)
        {
            text += $"\n-{condition.Description}";
        }
        GetComponent<Text>().text = text;
    }
}
