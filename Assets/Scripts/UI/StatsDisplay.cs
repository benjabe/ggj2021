using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _statDisplayEntryPrefab = null;

    private void Awake()
    {
        // Populate stat list
        foreach (var stat in Ship.GetStatNames())
        {
            Instantiate(_statDisplayEntryPrefab, transform).name = stat;
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateStatTexts();
    }

    private void UpdateStatTexts()
    {
        foreach (Transform entry in transform)
        {
            var text = entry.GetComponent<Text>();
            if (text != null)
            {
                text.text = $"{entry.name}: {Ship.GetStatValue(entry.name)}";
            }
        }
    }
}
