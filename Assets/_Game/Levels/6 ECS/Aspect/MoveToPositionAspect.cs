using Unity.Entities;
using Unity.Transforms;

public readonly partial struct MoveToPositionAspect : IAspect
{
    private readonly TransformAspect _transformAspect;
    private readonly RefRO<TargetPositionComponent> _targetPosition;

    public void Move(float time)
    {
        var (pos, rot) = _transformAspect.LocalPosition.CalculatePosBurst(_targetPosition.ValueRO.Value.y, time);

        _transformAspect.LocalRotation = rot;
        _transformAspect.LocalPosition = pos;
    }
}