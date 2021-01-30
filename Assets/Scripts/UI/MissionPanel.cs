using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour
{
    [SerializeField] private bool _useDebugMissionCards = false;
    [SerializeField] private GameObject _missionCardPrefab = null;

    private List<MissionEvent> _missions;

    private void Start()
    {
        if (_useDebugMissionCards)
        {
            PopulateDebugMissions();
        }
        PopulateCards();
    }

    private void PopulateCards()
    {
        foreach (var mission in _missions)
        {
            var go = Instantiate(_missionCardPrefab, transform);
            go.GetComponent<MissionCard>().MissionEvent = mission;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        GetComponent<VerticalLayoutGroup>().enabled = false;
        GetComponent<VerticalLayoutGroup>().enabled = true;
    }

    private void PopulateDebugMissions()
    {
        _missions = new List<MissionEvent>()
        {
            new MissionEvent(
                new List<Condition>()
                {
                    new Condition(() => Ship.GetStatValue("O2") > 0.5f, "O2 > 50%"),
                },
                "Have enough oxygen",
                3
            )
        };
    }
}
