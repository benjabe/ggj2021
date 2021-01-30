using UnityEngine;
using UnityEngine.UI;

public class JobQueueEntry : MonoBehaviour
{
    [SerializeField] private Text _jobNameText = null;
    [SerializeField] private ProgressBar _progressBar = null;
    [SerializeField] private Button _cancelButton = null;

    public Job Job { get; set; }

    private void Start()
    {
        _jobNameText.text = Job.Name;
        _cancelButton.onClick.AddListener(Cancel);
    }

    private void Update()
    {
        _progressBar.SetPercentage(Job.CurrentWork / Job.RequiredWork);
    }

    private void Cancel()
    {
        Job.CancelJob(Job);
    }
}
