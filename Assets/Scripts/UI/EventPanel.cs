using UnityEngine;
using UnityEngine.UI;

public class EventPanel : MonoBehaviour
{
    [SerializeField] private Text _nameText = null;
    [SerializeField] private Text _descriptionText = null;
    [SerializeField] private Button _okButton = null;

    private void Awake()
    {
        Time.timeScale = 0;
        _okButton.onClick.AddListener(() => { Destroy(gameObject); });
    }

    public void SetText(string nameText, string descriptionText)
    {
        _nameText.text = "<b>" + nameText + "</b>";
        _descriptionText.text = descriptionText;
    }
}
