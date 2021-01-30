using System;
using System.Collections.Generic;
using UnityEngine;

public class JobQueuePanel : MonoBehaviour
{
    [SerializeField] private GameObject _jobQueueEntryPrefab = null;

    private Dictionary<Job, GameObject> _jobToEntry = new Dictionary<Job, GameObject>();

    private void Awake()
    {
        Job.OnJobCompleted += OnJobCompleted;
        Job.OnJobCancelled += OnJobCancelled;
        Job.OnJobQueued += OnJobQueued;
    }

    private void OnJobQueued(Job job)
    {
        // Add an entry
        AddEntry(job);
    }

    private void OnJobCompleted(Job job)
    {
        // Remove an entry. In the future, maybe store completed jobs? Maybe. Who knows.
        Destroy(_jobToEntry[job]);
        _jobToEntry.Remove(job);
    }

    private void OnJobCancelled(Job job)
    {
        // Remove an entry. In the future, maybe store completed jobs? Maybe. Who knows.
        Destroy(_jobToEntry[job]);
        _jobToEntry.Remove(job);
    }

    private void AddEntry(Job job)
    {
        var go = Instantiate(_jobQueueEntryPrefab, transform);
        go.GetComponent<JobQueueEntry>().Job = job;
        _jobToEntry.Add(job, go);
    }
}
