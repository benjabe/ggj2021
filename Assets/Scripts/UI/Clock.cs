using System;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private TimeSystem _timeSystem;
    private Text _text;

    private void Awake()
    {
        _text = GetComponentInChildren<Text>();
        _timeSystem = FindObjectOfType<TimeSystem>();
        _timeSystem.OnSecondChanged += OnSecondChanged;
        _text.text = _timeSystem.ToString();
    }

    private void OnSecondChanged()
    {
        _text.text = _timeSystem.ToString();
    }
}
