using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    [SerializeField] private GameObject _helpPanel = null;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => Instantiate(_helpPanel, GameObject.Find("Canvas").transform));
    }
}
