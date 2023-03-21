using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTools : MonoBehaviour
{
    public const float DEPTH_OFFSET = 7;
    public const float HEIGHT_SCALE = 1.5f;
    public const float NOISE_SCALE = .2f;
    private const int SIDE_LENGTH = 100;
    private const int FPS_SAMPLE_COUNT = 20;
    public static SceneTools Instance;

    public static readonly quaternion RotGoal = quaternion.Euler(130, 50, 150);

    [SerializeField] private TMP_Text _cubeCountText, _fpsText, _nameText;

    private readonly int[] _fpsSamples = new int[FPS_SAMPLE_COUNT];
    private int _sampleIndex;

    [field: SerializeField] public Color[] ColorArray { get; private set; }

    private static int Depth { get; set; } = 1;

    public static Vector3 CubeScale { get; } = new(0.7f, 0.7f, 0.7f);
    public static int GetCount => SIDE_LENGTH * SIDE_LENGTH * Depth;

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

        // I actually hate this
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

    public void SetNameText(string testName) => _nameText.text = $"{SceneManager.GetActiveScene().buildIndex + 1} - {testName}";

    private void UpdateFps()
    {
        var sum = 0;
        for (var i = 0; i < FPS_SAMPLE_COUNT; i++)
        {
            sum += _fpsSamples[i];
        }

        _fpsText.text = $"FPS: {sum / FPS_SAMPLE_COUNT}";
    }

    public static void LoopPositions(Action<int, Vector3> action)
    {
        var i = 0;
        for (var y = 0; y < Depth; y++)
        {
            for (var x = 0; x < SIDE_LENGTH; x++)
            {
                for (var z = 0; z < SIDE_LENGTH; z++)
                {
                    action(i++, new Vector3(x, y, z));
                }
            }
        }
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
        pos.y = SceneTools.HEIGHT_SCALE * noise.cnoise(new float2(pos.x * SceneTools.NOISE_SCALE + time, pos.z * SceneTools.NOISE_SCALE + time)) +
                yOffset * SceneTools.DEPTH_OFFSET;
        var rot = math.nlerp(quaternion.identity, SceneTools.RotGoal, t);
        return (pos, rot);
    }
}