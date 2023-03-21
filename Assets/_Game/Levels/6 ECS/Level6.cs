using Unity.Entities;
using UnityEngine;

public class Level6 : MonoBehaviour
{
    private PlayerSpawnerSystem _system;
    private SystemHandle _system2;
    private World _world;

    private void Start()
    {
        Invoke(nameof(DelayedSystems), 0.5f);

        SceneTools.Instance.SetCountText(SceneTools.GetCount);
        SceneTools.Instance.SetNameText("ECS + Jobs + Burst");
    }

    private void Update()
    {
        if (_system == null) return;
        _system.Update();
        _system2.Update(_world.Unmanaged);
    }

    private void OnDestroy()
    {
        _world?.DestroySystemManaged(_system);
        _world?.DestroySystem(_system2);
    }

    private void DelayedSystems()
    {
        _world = World.DefaultGameObjectInjectionWorld;

        _system = _world.CreateSystemManaged<PlayerSpawnerSystem>();
        _system2 = _world.CreateSystem<MovingISystem>();
    }
}