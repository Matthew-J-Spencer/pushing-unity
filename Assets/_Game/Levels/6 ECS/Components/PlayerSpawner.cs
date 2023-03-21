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
        AddComponent(new PlayerSpawnerComponent
        {
            PlayerPrefab = GetEntity(authoring.PlayerPrefab)
        });
    }
}