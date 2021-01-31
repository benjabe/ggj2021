using UnityEngine;

public class RepairComponentJob : Job
{
    private static float _timePerConditionPoint = 60.0f * 45.0f / 100.0f; // 45 minutes to full repair, divided by 100 for each point
    public ShipSystem System { get; private set; }
    public ShipSystemComponent Component { get; private set; }

    public override string Name => "Repair Component";

    public RepairComponentJob(ShipSystem system, ShipSystemComponent component)
    {
        System = system;
        Component = component;
        _workPosition = System.WorkPosition;
        System.OnWorkPositionChanged += OnSystemWorkPositionChanged;
        _workEfficiencyMultiplier = 1 / (system.RepairTimeMultiplier * component.RepairTimeMultiplier * _timePerConditionPoint);
        _requiredWork = 100.0f;
        _currentWork = Component.Condition;
    }

    public override bool CheckInstantiationPrerequisite()
    {
        return System != null && Component != null && System.HasComponentOfType(Component.Data);
    }

    public override void ExecuteJobPostcondition(Astronaut astronaut)
    {
        // Nothing, effect is gradual during the job
    }

    private void OnSystemWorkPositionChanged(Vector3 position)
    {
        _workPosition = position;
    }

    public override void ExecuteJobPerformanceEffect(Astronaut astronaut, float astronautEfficiency, float workDone)
    {
        Component.Repair(workDone);
        _currentWork = Component.Condition;
    }

    public override bool CheckPerformJobPrerequisite(Astronaut astronaut, float astronautEfficiency)
    {
        return CheckInstantiationPrerequisite();
    }
}
