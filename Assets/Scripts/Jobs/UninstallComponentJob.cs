﻿using UnityEngine;

public class UninstallComponentJob : Job
{
    public ShipSystem System { get; private set; }
    public ShipSystemComponent Component { get; private set; }

    public override string Name => "Uninstall Component";

    public UninstallComponentJob(ShipSystem system, ShipSystemComponent component)
    {
        System = system;
        Component = component;
        _workPosition = System.WorkPosition;
        System.OnWorkPositionChanged += OnSystemWorkPositionChanged;
        _workEfficiencyMultiplier = 1 / (system.UninstallTimeMultiplier * component.UninstallTimeMultiplier);
        _requiredWork = 10.0f;
    }

    public override bool CheckPrerequisite()
    {
        return System != null && Component != null && System.HasComponent(Component);
    }

    public override void ExecuteJobPostcondition(Astronaut astronaut)
    {
        // Remove the component from the system.
        System.RemoveSystemComponent(Component);
        System.OnWorkPositionChanged -= OnSystemWorkPositionChanged;
    }

    private void OnSystemWorkPositionChanged(Vector3 position)
    {
        _workPosition = position;
    }
}
