using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MathTest : MonoBehaviour
{
    [SerializeField] private bool _useMathematics;
    [SerializeField] private int _iterations = 1000000;
    [SerializeField] private TestType _type;

    public enum TestType
    {
        InverseLerp,
        QuaternionLerp,
        Perlin
    }

    void Update()
    {
        switch (_type)
        {
            case TestType.InverseLerp:
                if (_useMathematics)
                {
                    for (int i = 0; i < _iterations; i++)
                    {
                        var t = math.unlerp(1, 50, 25);
                    }
                }
                else
                {
                    for (int i = 0; i < _iterations; i++)
                    {
                        var t = Mathf.InverseLerp(1, 50, 25);
                    }
                }

                break;
            case TestType.QuaternionLerp:
                if (_useMathematics)
                {
                    for (int i = 0; i < _iterations; i++)
                    {
                        var rot = math.slerp(quaternion.identity, SceneTools.RotGoal, 0.5f);
                    }
                }
                else
                {
                    for (int i = 0; i < _iterations; i++)
                    {
                        var rot = Quaternion.Slerp(Quaternion.identity, SceneTools.RotGoal, 0.5f);
                    }
                }

                break;
            case TestType.Perlin:
                if (_useMathematics)
                {
                    for (int i = 0; i < _iterations; i++)
                    {
                        var t = noise.cnoise(new float2(20, 50));
                    }
                }
                else
                {
                    for (int i = 0; i < _iterations; i++)
                    {
                        var t = Mathf.PerlinNoise(20, 50);
                    }
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}