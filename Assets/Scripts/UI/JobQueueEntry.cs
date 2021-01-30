using UnityEngine;
using UnityEngine.UI;

public class JobQueueEntry : MonoBehaviour
{
    [SerializeField] private Text _jobNameText = null;
    [SerializeField] private ProgressBar _progressBar = null;

    public Job Job { get; set; }

    private void Start()
    {
        _jobNameText.text = Job.Name;
    }

    private void Update()
    {
        _progressBar.SetPercentage(Job.CurrentWork / Job.RequiredWork);
    }
}
