﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionEvent
{
    //Name of the event
    public string Name;
    //The conditions that need to be true for the event to complete
    public List<Condition> Conditions;
    public int TimeOfEvent { get; private set; }
    public static Action<MissionEvent> OnMissionComplete;
    public static Action<MissionEvent> OnMissionFailed;

    public MissionEvent(List<Condition> conditions, string name, int time)
    {
        Conditions = conditions;
        Name = name;
        TimeOfEvent = time;
        TimeSystem.Instance.OnSecondChanged += () =>
        {
            if (TimeOfEvent == TimeSystem.Instance.SecondsSinceStart)
            {
                if (TestIfConditionsMet())
                {
                    Debug.Log($"Mission: {Name} Succesful!");
                    OnMissionComplete?.Invoke(this);
                }
                else
                {

                    Debug.Log($"Mission: {Name} failed :(");
                    OnMissionFailed?.Invoke(this);
                }

            }
        };
    }

    public bool TestIfConditionsMet()
    {
        Debug.Log($"Testing MissionEvent: {Name}");
        foreach (Condition condition in Conditions)
        {
            Debug.Log($"{condition.Description}: {condition.Heuristic()}");
            if (condition.Heuristic() == false)
            {
                return false;
            }
        }
        return true;
    }
}