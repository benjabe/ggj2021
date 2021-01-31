using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider _slider = null;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(OnVolumeChanged);
        OnVolumeChanged(_slider.value);
    }

    private void OnVolumeChanged(float value)
    {
        SoundManager.Volume = value;
    }
}
