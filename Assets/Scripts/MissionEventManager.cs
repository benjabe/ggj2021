using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEventManager : MonoBehaviour
{
    public Dictionary<MissionEvent, bool> MissionEventList = new Dictionary<MissionEvent, bool>();
    [SerializeField]
    private GameObject _missionEntryPrefab;
    [SerializeField]
    private GameObject _missionOverview;

    private void Awake()
    {
        MissionEvent.OnMissionComplete += OnMissionComplete;
    }

    private void Start()
    {
        Condition condition = new Condition(() => { return true; }, "Test condition");
        List<Condition> condList = new List<Condition>();
        condList.Add(condition);

        CreateMissionEntry(new MissionEvent(condList, "Enter Lunar Orbit", 20));


        List<Condition> asteroidList = new List<Condition>();
        //Enter lunar orbit
        //deflect yuge asteroid
        //Land on moon


    }

    void CheckIfAllMissionsAreComplete()
    {
        bool weDidIt = true;
        foreach (MissionEvent mission in MissionEventList.Keys)
        {
            if (MissionEventList[mission] == false)
            {
                weDidIt = false;
                Debug.Log($"Mission: {mission.Name} was false.");
                break;
            }
        }

        if (weDidIt)
        {
            Debug.Log("You Win!");
        }
    }

    /// <summary>
    /// Checks if we beat the mission and then checks if we have beaten all missions
    /// </summary>
    /// <param name="mission"></param>
    void OnMissionComplete(MissionEvent mission)
    {
        MissionEventList[mission] = true;
        CheckIfAllMissionsAreComplete();
    }

    void CreateMissionEntry(MissionEvent mission)
    {
        var newEntry = Instantiate(_missionEntryPrefab, _missionOverview.transform);
        var card = newEntry.GetComponent<MissionCard>();
        card.MissionEvent = mission;
        MissionEventList.Add(mission, false);
        //create the UI entry
        //add it to a list

    }
}
