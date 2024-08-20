using Unity.Entities;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject PlayerPrefab;
}

public struct PlayerSpawnerComponent : IComponentData
{
    public Entity PlayerPrefab;
}


public class PlayerSpawnerBaker : Baker<PlayerSpawner>
{
    public override void Bake(PlayerSpawner authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new PlayerSpawnerComponent
        {
            PlayerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic)
        });
    }
}