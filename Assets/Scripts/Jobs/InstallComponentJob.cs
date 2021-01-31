using UnityEngine;

public class InstallComponentJob : Job
{
    public ShipSystem System { get; private set; }
    public ShipSystemComponent Component { get; private set; }

    public override string Name => "Install Component";

    public InstallComponentJob(ShipSystem system, ShipSystemComponent component)
    {
        System = system;
        Component = component;
        _workPosition = System.WorkPosition;
        System.OnWorkPositionChanged += OnSystemWorkPositionChanged;
        _workEfficiencyMultiplier = 1 / (system.InstallTimeMultiplier * component.InstallTimeMultiplier);
        _requiredWork = 60.0f * 10.0f;
    }

    public override bool CheckInstantiationPrerequisite()
    {
        return System != null && !(System is Astronaut) && Component != null && System.CanAddComponent(Component);
    }

    public override void ExecuteJobPostcondition(Astronaut astronaut)
    {
        System.AddSystemComponent(Component);
    }

    private void OnSystemWorkPositionChanged(Vector3 position)
    {
        _workPosition = position;
    }

    public override void ExecuteJobPerformanceEffect(Astronaut astronaut, float astronautEfficiency, float workDone)
    {
        // Don't need anything here, methinks.
    }

    public override bool CheckPerformJobPrerequisite(Astronaut astronaut, float astronautEfficiency)
    {
        return CheckInstantiationPrerequisite() && astronaut.HasComponent(Component);
    }
}
