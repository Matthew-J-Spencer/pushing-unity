using System;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneTools : MonoBehaviour
{
    public static SceneTools Instance;

    [SerializeField] private TMP_Text _cubeCountText, _fpsText, _nameText;

    [field: SerializeField] public Color[] ColorArray { get; private set; }

    public const float DEPTH_OFFSET = 7;
    public const float HEIGHT_SCALE = 3.5f;
    public const float NOISE_SCALE = .2f;
    public const int SIDE_LENGTH = 100;
    private const int FPS_SAMPLE_COUNT = 20;

    public static int Depth { get; private set; } = 1;

    public static Vector3 CubeScale { get; } = new(0.7f, 0.7f, 0.7f);

    private readonly int[] _fpsSamples = new int[FPS_SAMPLE_COUNT];
    private int _sampleIndex = 0;

    public static readonly quaternion RotGoal = quaternion.Euler(130, 50, 50);

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = -1;

        InvokeRepeating(nameof(UpdateFps), 0, 0.1f);
    }

    private void Update()
    {
        _fpsSamples[_sampleIndex++] = (int)(1.0f / Time.deltaTime);
        if (_sampleIndex >= FPS_SAMPLE_COUNT) _sampleIndex = 0;

        if (Input.GetKeyDown(KeyCode.Alpha1)) Act(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Act(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Act(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Act(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Act(5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) Act(6);
        if (Input.GetKeyDown(KeyCode.Alpha7)) Act(7);
        if (Input.GetKeyDown(KeyCode.Alpha8)) Act(8);
        if (Input.GetKeyDown(KeyCode.Alpha9)) Act(9);
    }

    private static void Act(int num)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Depth = num;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(num - 1);
        }
    }

    public void SetCountText(int count) => _cubeCountText.text = $"Cube Count: {count:n0}";
    public static int GetCount => SIDE_LENGTH * SIDE_LENGTH * Depth;

    public void SetNameText(string testName) => _nameText.text = $"{SceneManager.GetActiveScene().buildIndex + 1} - {testName}";

    private void UpdateFps()
    {
        var sum = 0;
        for (var i = 0; i < FPS_SAMPLE_COUNT; i++) sum += _fpsSamples[i];

        _fpsText.text = $"FPS: {sum / FPS_SAMPLE_COUNT}";
    }
}

public static class CubeHelpers
{
    public static (Vector3 pos, Quaternion rot) CalculatePos(this Vector3 pos, float yOffset, float time)
    {
       return ((float3)pos).CalculatePos(yOffset, time);
    }

    public static (float3 pos, Quaternion rot) CalculatePos(this float3 pos, float yOffset, float time)
    {
        var t = math.unlerp(yOffset, SceneTools.HEIGHT_SCALE + yOffset, pos.y);
        pos.y = SceneTools.HEIGHT_SCALE * noise.cnoise(new float2(pos.x * SceneTools.NOISE_SCALE + time, pos.z * SceneTools.NOISE_SCALE + time)) + yOffset;
        var rot = math.nlerp(quaternion.identity, SceneTools.RotGoal,t);
        return (pos, rot);
    }
}