using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEventManager : MonoBehaviour
{
    public static Dictionary<MissionEvent, bool> MissionEvents = new Dictionary<MissionEvent, bool>();
    public static Action OnMissionsCreated;
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
        CreateMissionEntry(new MissionEvent(new List<Condition>()
        {
            new Condition(()=>FindObjectOfType<EngineSystem>().AllComponentConditionsAbove(85), "All Engine component  >85% condition")
        }, "Fly towards the Moon", 3600));
        //fly towards moon
        //engines 85%
        CreateMissionEntry(new MissionEvent(new List<Condition>()
        {
            new Condition(()=>FindObjectOfType<EngineSystem>().AllComponentConditionsAbove(60), "All Engine component >60% condition"),
            new Condition(()=>FindObjectOfType<GuidanceComputer>().AverageComponentConditionAbove(75), "Average Guidance computer component condition >75%" )
        }, "Enter lunar orbit", 86400));
        //Enter lunar orbit
        //engines 60%
        //guidance computer 75%
        CreateMissionEntry(new MissionEvent(new List<Condition>()
        {
            new Condition(()=>FindObjectOfType<AsteroidDeflectionSystem>().AverageComponentConditionAbove(90), "Asteroid Deflection at 90% average condition")
        }, "Deflect incoming asteroid", 120000));
        //deflect yuge asteroid
        //asteroid deflection thingy 90%
        CreateMissionEntry(new MissionEvent(new List<Condition>()
        {
            new Condition(()=>FindObjectOfType<EngineSystem>().AllComponentConditionsAbove(40), "All Engine component >40% condition"),
            new Condition(()=>FindObjectOfType<GuidanceComputer>().AverageComponentConditionAbove(90), "Average Guidance computer component condition >90%" )
        }, "Land on the Moon", 172800));
        //Land on moon
        //engines 40
        //guidance computer 90%
        CreateMissionEntry(new MissionEvent(new List<Condition>()
        {
            new Condition(()=>FindObjectOfType<EngineSystem>().AllComponentConditionsAbove(85), "All Engine component  >85% condition")
        }, "Fly back to the Earth", 200000));
        //fly back to earth
        //guidance computer 75%
        //engines 70%
        CreateMissionEntry(new MissionEvent(new List<Condition>()
        {
            new Condition(()=>FindObjectOfType<EngineSystem>().AllComponentConditionsAbove(80), "All Engine component >80% condition"),
            new Condition(()=>FindObjectOfType<GuidanceComputer>().AverageComponentConditionAbove(70), "Average Guidance computer component condition >70%" )
        }, "Land on the Earth", 220000));
        //land on earth
        //engines 80
        //guidance computer 70%

        OnMissionsCreated?.Invoke();

    }

    void CheckIfAllMissionsAreComplete()
    {
        bool weDidIt = true;
        foreach (MissionEvent mission in MissionEvents.Keys)
        {
            if (MissionEvents[mission] == false)
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
        MissionEvents[mission] = true;
        CheckIfAllMissionsAreComplete();
    }

    void CreateMissionEntry(MissionEvent mission)
    {
        //var newEntry = Instantiate(_missionEntryPrefab, _missionOverview.transform);
        //var card = newEntry.GetComponent<MissionCard>();
        //card.MissionEvent = mission;
        MissionEvents.Add(mission, false);
    }
}
