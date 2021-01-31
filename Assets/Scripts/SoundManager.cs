using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance = null;
    private static float _volume = 1.0f;
    private static float _timeBetweenCleanup = 5.0f;
    private static float _nextCleanupTime = 5.0f;

    public static float Volume
    {
        get => _volume;
        set
        {
            _volume = value;
            OnVolumeChanged(value);
        }
    }

    private static Dictionary<AudioSource, float> _sources = new Dictionary<AudioSource, float>();

    private void Awake()
    {
        _instance = this;
        //PlaySound("TestMusic", true);
    }

    private void Update()
    {
        if (_nextCleanupTime <= Time.unscaledTime)
        {
            foreach (var source in _sources.Keys.ToList())
            {
                if (_sources[source] < Time.unscaledTime)
                {
                    Destroy(source);
                    _sources.Remove(source);
                }
            }
            _nextCleanupTime = Time.unscaledTime + _timeBetweenCleanup;
        }
    }

    public static void PlaySound(string soundName, bool repeating = false)
    {
        var clip = Resources.Load<AudioClip>($"Audio/{soundName}");
        var source = new GameObject().AddComponent<AudioSource>();
        Instantiate(source.gameObject, _instance.transform);
        source.clip = clip;
        source.volume = Volume;
        source.Play();
        source.loop = repeating;
        _sources.Add(source, clip.length + 5.0f);
    }

    public static void OnVolumeChanged(float volume)
    {
        foreach (var source in _sources.Keys)
        {
            source.volume = volume;
        }
    }
}