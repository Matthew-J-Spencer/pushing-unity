using Unity.Mathematics;
using UnityEngine;

public class Level1Cube : MonoBehaviour
{
    private float _yOffset;

    public void Init(float yOffset) => _yOffset = yOffset;

    private void Update()
    {
        var (pos, rot) = transform.position.CalculatePos(_yOffset, Time.time);
        transform.SetPositionAndRotation(pos, rot);
    }
}