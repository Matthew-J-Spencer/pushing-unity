using Unity.Burst;
using Unity.Entities;
using UnityEngine.SceneManagement;

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
        if(World.All.Count <= 0) return;
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