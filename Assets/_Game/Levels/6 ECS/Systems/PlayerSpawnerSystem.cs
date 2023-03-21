using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

[BurstCompile]
[DisableAutoCreation]
public partial class PlayerSpawnerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // This is bad code. Can somebody tell me how to instantiate entities at a position using a command buffer?
        if (EntityManager.CreateEntityQuery(typeof(TargetPositionComponent)).CalculateEntityCount() != 0)
        {
            foreach (var (aspect, targetPos) in SystemAPI.Query<TransformAspect, TargetPositionComponent>())
            {
                aspect.LocalPosition = targetPos.Value;
            }

            Enabled = false;
            return;
        }

        var buffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        for (var y = 0; y < SceneTools.Depth; y++)
        {
            for (var x = 0; x < SceneTools.SIDE_LENGTH; x++)
            {
                for (var z = 0; z < SceneTools.SIDE_LENGTH; z++)
                {
                    var entity = buffer.Instantiate(SystemAPI.GetSingleton<PlayerSpawnerComponent>().PlayerPrefab);
                    buffer.SetComponent(entity, new TargetPositionComponent
                    {
                        Value = new float3(x, y, z)
                    });
                }
            }
        }
    }
}