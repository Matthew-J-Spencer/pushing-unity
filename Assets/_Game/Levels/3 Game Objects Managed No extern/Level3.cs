using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class Level3 : MonoBehaviour
{
    [SerializeField] private Transform _cubePrefab;

    private Transform[] _spawnedCubes;
    private Vector3[] _lastPositions;
    private float[] _cubeYOffsets;

    private void Start()
    {
        var count = SceneTools.GetCount;
        _spawnedCubes = new Transform[count];
        _lastPositions = new Vector3[count];
        _cubeYOffsets = new float[count];

        var i = 0;
        for (var y = 0; y < SceneTools.Depth; y++)
        {
            for (var x = 0; x < SceneTools.SIDE_LENGTH; x++)
            {
                for (var z = 0; z < SceneTools.SIDE_LENGTH; z++)
                {
                    var startPos = new Vector3(x, y, z);
                    _cubeYOffsets[i] = y;
                    _lastPositions[i] = startPos;
                    _spawnedCubes[i++] = Instantiate(_cubePrefab, startPos, Quaternion.identity, transform);
                }
            }
        }

        SceneTools.Instance.SetCountText(count);
        SceneTools.Instance.SetNameText("Managed Cubes - Avoid extern");
    }

    private void Update()
    {
        var time = Time.time;
        for (var i = 0; i < _spawnedCubes.Length; i++)
        {
            var (pos, rot) = _lastPositions[i].CalculatePos(_cubeYOffsets[i], time);

            _lastPositions[i] = pos;

            _spawnedCubes[i].SetPositionAndRotation(pos, rot);
        }
    }
}