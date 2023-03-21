using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTools : MonoBehaviour
{
    public const float DEPTH_OFFSET = 7;

    // Variance between mathf & mathematics perlin. Trying to keep them visually similar
    public const float HEIGHT_SCALE = 2.5f;
    public const float BURST_HEIGHT_SCALE = 1.5f;

    public const float NOISE_SCALE = .2f;
    private const int SIDE_LENGTH = 100;
    private const int FPS_SAMPLE_COUNT = 20;
    public static SceneTools Instance;

    public static readonly quaternion RotGoal = quaternion.Euler(130, 50, 150);

    [SerializeField] private TMP_Text _cubeCountText, _fpsText, _nameText;
    [SerializeField] private Slider _slider;

    private readonly int[] _fpsSamples = new int[FPS_SAMPLE_COUNT];
    private int _sampleIndex;
    private int _cachedDepth;

    [field: SerializeField] public Color[] ColorArray { get; private set; }

    private static int Depth { get; set; } = 1;

    public static Vector3 CubeScale { get; } = new(0.7f, 0.7f, 0.7f);
    public static int GetCount => SIDE_LENGTH * SIDE_LENGTH * Depth;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = -1;

        _cachedDepth = Depth;
        ApplyCountUpdate(Depth, true);
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

        if (Input.GetMouseButtonUp(0) && Depth != _cachedDepth)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private static void Act(int num) => SceneManager.LoadScene(num - 1);

    public void SetCountText(int count) => _cubeCountText.text = $"Count: {count:n0}";

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

    public void UpdateCount(float val)
    {
        ApplyCountUpdate(Mathf.CeilToInt(val));
    }

    private void ApplyCountUpdate(int val, bool applySliderChange = false)
    {
        Depth = val;
        if (applySliderChange) _slider.value = val;
    }
}

public static class CubeHelpers
{
    public static (Vector3 pos, Quaternion rot) CalculatePos(this Vector3 pos, float yOffset, float time)
    {
        var t = Mathf.InverseLerp(yOffset, SceneTools.HEIGHT_SCALE + yOffset, pos.y);
        var rot = Quaternion.Slerp(quaternion.identity, SceneTools.RotGoal, t);
        pos.y = SceneTools.HEIGHT_SCALE * Mathf.PerlinNoise(pos.x * SceneTools.NOISE_SCALE + time, pos.z * SceneTools.NOISE_SCALE + time) + yOffset * SceneTools.DEPTH_OFFSET;
        return (pos, rot);
    }

    public static (float3 pos, Quaternion rot) CalculatePosBurst(this float3 pos, float yOffset, float time)
    {
        var t = math.unlerp(yOffset, SceneTools.BURST_HEIGHT_SCALE + yOffset, pos.y);
        pos.y = SceneTools.BURST_HEIGHT_SCALE * noise.cnoise(new float2(pos.x * SceneTools.NOISE_SCALE + time, pos.z * SceneTools.NOISE_SCALE + time)) +
                yOffset * SceneTools.DEPTH_OFFSET;
        var rot = math.nlerp(quaternion.identity, SceneTools.RotGoal, t);
        return (pos, rot);
    }
}