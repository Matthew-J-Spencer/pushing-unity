using Unity.Entities;
using Unity.Transforms;

public readonly partial struct MoveToPositionAspect : IAspect
{
    private readonly RefRW<LocalTransform> _localTransform;
    private readonly RefRO<TargetPositionComponent> _targetPosition;

    public void Move(float time)
    {
        var (pos, rot) = _localTransform.ValueRO.Position.CalculatePosBurst(_targetPosition.ValueRO.Value.y, time);

        _localTransform.ValueRW.Position = pos;
        _localTransform.ValueRW.Rotation = rot;
    }
}