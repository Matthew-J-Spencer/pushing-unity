using System;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;

    private void Start()
    {
        for (var y = 0; y < SceneTools.Depth; y++)
        {
            for (var x = 0; x < SceneTools.SIDE_LENGTH; x++)
            {
                for (var z = 0; z < SceneTools.SIDE_LENGTH; z++)
                {
                    Instantiate(_cubePrefab, new Vector3(x, y * SceneTools.DEPTH_OFFSET, z), Quaternion.identity, transform)
                        .AddComponent<Level1Cube>()
                        .Init(y);
                }
            }
        }

        SceneTools.Instance.SetCountText(SceneTools.GetCount);
        SceneTools.Instance.SetNameText("Individual MonoBehaviours");
    }
}