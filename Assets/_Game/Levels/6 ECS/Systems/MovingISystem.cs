using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[DisableAutoCreation]
public partial struct MovingISystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new MoveJob
        {
            Time = (float)SystemAPI.Time.ElapsedTime
        }.ScheduleParallel(state.Dependency).Complete();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}

[BurstCompile]
public partial struct MoveJob : IJobEntity
{
    public float Time;

    [BurstCompile]
    private void Execute(MoveToPositionAspect aspect)
    {
        aspect.Move(Time);
    }
}