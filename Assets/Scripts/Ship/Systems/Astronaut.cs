using UnityEngine;

public class Astronaut : ShipSystem
{
    [SerializeField] private float _moveSpeed = 1.0f;

    private Job _currentJob = null;

    public override void UpdateAccordingToWorkingComponentCount(int count)
    {
        WorkPosition = transform.position;
        if (_currentJob == null)
        {
            // No job, try to pop one off the queue
            if (Job.GetJobQueueCount() > 0)
            {
                _currentJob = Job.DequeueJob();
                Debug.Log("Astronaut got a new job.");
            }
        }
        if (_currentJob != null)
        {
            // Go to where the job is
            var dist = _currentJob.WorkPosition - transform.position;
            var dir = dist.normalized;
            var toMove = dir * Time.deltaTime * _moveSpeed;
            if (toMove.magnitude >= dist.magnitude)
                transform.position = _currentJob.WorkPosition;
            else
                transform.position += toMove;
            if (Vector3.Distance(transform.position, _currentJob.WorkPosition) < 0.05f)
            {
                // Perform work
                var jobCompleted = _currentJob.PerformJob(this, 1.0f);
                if (jobCompleted)
                {
                    Debug.Log("Completed job!");
                    _currentJob = null;
                }
            }
        }
    }
}
