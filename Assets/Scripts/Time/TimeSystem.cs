using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public TimeSystem Instance;

    public Action OnSecondChanged;
    public Action OnMinuteChanged;
    public Action OnHourChanged;
    public Action OnDayChanged;
    public int SecondsSinceStart { get; private set; } = 0;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        if (Instance == null)
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TickSecond();
        }
    }

    /// <summary>
    /// Increases the second counter by 1
    /// </summary>
    private void TickSecond()
    {
        SecondsSinceStart += 111;
        OnSecondChanged?.Invoke();
        if (SecondsSinceStart % 60 == 0)
        {
            OnMinuteChanged?.Invoke();
        }
        if (SecondsSinceStart % (60 * 60) == 0)
        {
            OnHourChanged?.Invoke();
        }
        if (SecondsSinceStart % (60 * 60 * 24) == 0)
        {
            OnDayChanged?.Invoke();
        }

        Debug.Log(Format());
    }

    /// <summary>
    /// Formats the current time as DD:HH:MM:SS
    /// </summary>
    /// <returns></returns>
    public string Format()
    {
        int day = (int)Mathf.Floor(SecondsSinceStart / 86400);
        int hour = (int)Mathf.Floor(SecondsSinceStart / 3600);
        int minute = (int)Mathf.Floor(SecondsSinceStart / 60) % 60;
        int second = SecondsSinceStart % 60;

        string timeStr = ($"{(day < 10 ? "0" : (day / 10).ToString())}{day % 10}:{(hour < 10 ? "0" : (hour / 10).ToString())}{hour % 10}:{(minute < 10 ? "0" : (minute / 10).ToString())}{minute % 10}:{(second < 10 ? "0" : (second / 10).ToString())}{second % 10}");
        return timeStr;
    }
}
