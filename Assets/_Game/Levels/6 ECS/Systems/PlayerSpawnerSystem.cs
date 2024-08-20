using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[DisableAutoCreation]
public partial class PlayerSpawnerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // This is bad code. Can somebody tell me how to instantiate entities at a position using a command buffer?
        if (EntityManager.CreateEntityQuery(typeof(TargetPositionComponent)).CalculateEntityCount() != 0)
        {
            foreach (var (aspect, targetPos) in SystemAPI.Query<RefRW<LocalTransform>, TargetPositionComponent>())
            {
                aspect.ValueRW.Position = targetPos.Value;
            }

            Enabled = false;
            return;
        }

        var buffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        var prefab = SystemAPI.GetSingleton<PlayerSpawnerComponent>().PlayerPrefab;
        SceneTools.LoopPositions((i, p) =>
        {
            var entity = buffer.Instantiate(prefab);
            buffer.SetComponent(entity, new TargetPositionComponent
            {
                Value = p
            });
        });
    }
}