using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

public class Level6 : MonoBehaviour
{
    private World _world;
    private PlayerSpawnerSystem _system;
    private SystemHandle _system2;

    private void Start()
    {
        Invoke(nameof(DelayedSystems), 0.5f);

        SceneTools.Instance.SetCountText(SceneTools.GetCount);
        SceneTools.Instance.SetNameText("ECS + Jobs + Burst");
    }

    void DelayedSystems()
    {
        _world = World.DefaultGameObjectInjectionWorld;

        _system = _world.CreateSystemManaged<PlayerSpawnerSystem>();
        _system2 = _world.CreateSystem<MovingISystem>();
    }

    private void OnDestroy()
    {
        _world?.DestroySystemManaged(_system);
        _world?.DestroySystem(_system2);
    }

    private void Update()
    {
        if (_system == null) return;
        _system.Update();
        _system2.Update(_world.Unmanaged);
    }
}