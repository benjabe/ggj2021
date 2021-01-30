using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform _fgRectTransform = null;
    [SerializeField] private RectTransform _bgRectTransform = null;

    public void SetPercentage(float percentage)
    {
        _fgRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _bgRectTransform.rect.width * percentage);
    }
}
