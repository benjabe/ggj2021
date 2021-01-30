﻿using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job
{
    private static Queue<Job> _jobQueue = new Queue<Job>();

    public static Action<Job> OnJobQueued { get; set; } = null;
    public static Action<Job> OnJobCompleted { get; set; } = null;

    /// <summary>
    /// The amount of work (baseline time in seconds) required for the job to be completed.
    /// </summary>
    protected float _requiredWork = 100.0f;
    /// <summary>
    /// The current amount of work that has been put into the job.
    /// </summary>
    protected float _currentWork = 0.0f;
    /// <summary>
    /// How much more efficient this job is in general, usually based on stuff like which systems or components are involved.
    /// </summary>
    protected float _workEfficiencyMultiplier = 1.0f;
    /// <summary>
    /// The position of the astronaut for the work to take place.
    /// </summary>
    protected Vector3 _workPosition = Vector3.zero;

    public float RequiredWork { get => _requiredWork; }
    public float CurrentWork { get => _currentWork; }
    public Vector3 WorkPosition { get => _workPosition; }


    /// <summary>
    /// The name of the job.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Checks if the job's prerequisites are met.
    /// </summary>
    /// <returns>Returns true if the prerequisites for the job to be worked on are present.</returns>
    public abstract bool CheckPrerequisite();
    /// <summary>
    /// Performs the job.
    /// </summary>
    /// <param name="astronaut">The astronaut doing the job.</param>
    /// <param name="astronautEfficiency">The efficiency at which the astronaut is performing the job. 0 = no progress. 1 = normal progress. Anything more is working faster than normal. Negative numbers are making things worse.</param>
    /// <returns>Return true if the job was completed.</returns>
    public bool PerformJob(Astronaut astronaut, float astronautEfficiency)
    {
        if (!CheckPrerequisite()) return false;
        _currentWork += astronautEfficiency * _workEfficiencyMultiplier * Time.deltaTime;
        if (_currentWork >= _requiredWork)
        {
            CompleteJob(astronaut);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Completes the job and makes whatever the result of the job is supposed to be happen.
    /// </summary>
    /// <param name="astronaut">The astronaut who completed the job.</param>
    public void CompleteJob(Astronaut astronaut)
    {
        ExecuteJobPostcondition(astronaut);
        OnJobCompleted?.Invoke(this);
    }

    /// <summary>
    /// Makes the thing that's supposed to happen upon job completion happen.
    /// </summary>
    /// <param name="astronaut">The astronaut who completed the job.</param>
    public abstract void ExecuteJobPostcondition(Astronaut astronaut);

    /// <summary>
    /// Enqueues a job to be performed by an astronaut.
    /// </summary>
    /// <param name="job">The job to enqueue.</param>
    public static void QueueJob(Job job)
    {
        _jobQueue.Enqueue(job);
        OnJobQueued?.Invoke(job);
    }
    /// <summary>
    /// Get the number of enqueued jobs.
    /// </summary>
    /// <returns>Tje number of enqueued jobs.</returns>
    public static int GetJobQueueCount()
    {
        return _jobQueue.Count;
    }
    /// <summary>
    /// Dequeues the next job. Removes the next job from the queue and returns it.
    /// </summary>
    /// <returns>The dequeued job.</returns>
    public static Job DequeueJob()
    {
        return _jobQueue.Dequeue();
    }
    /// <summary>
    /// Claims the next job for an astronaut. Dequeues the job.
    /// </summary>
    /// <returns>The dequeued job.</returns>
    public static Job ClaimJob(Astronaut astronaut)
    {
        return _jobQueue.Dequeue();
    }
}
