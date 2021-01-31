using UnityEngine;
using UnityEngine.UI;

public class CloseButtonListener : MonoBehaviour
{
    [SerializeField] private Button _closeButton = null;
    [SerializeField] private bool _destroyOnClose = false;

    private void Awake()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void Close()
    {
        if (!_destroyOnClose) gameObject.SetActive(false);
        else Destroy(gameObject);
    }
}