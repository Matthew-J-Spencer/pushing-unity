using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Level8 : MonoBehaviour
{
    [SerializeField] private Mesh _instanceMesh;
    [SerializeField] private Material _instanceMaterial;
    [SerializeField] private ComputeShader _compute;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _sliderValueText;
    
    [SerializeField] private Transform _pusher;
    [SerializeField] private float _pusherSpeed = 20;   
    
    private static int _countMultiplier = 1;
    private readonly uint[] _args = { 0, 0, 0, 0, 0 };
    private ComputeBuffer _argsBuffer;

    private int _count;

    private int _kernel;

    private ComputeBuffer _meshPropertiesBuffer;
    private int _cachedMultiplier;

    private void Start()
    {
        _kernel = _compute.FindKernel("cs_main");
        _count = SceneTools.GetCount * _countMultiplier;
        ApplyMultiplierUpdate(_countMultiplier, true);

        _argsBuffer = new ComputeBuffer(1, _args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        UpdateBuffers();

        SceneTools.Instance.SetCountText(_count);
        SceneTools.Instance.SetNameText("GPU Instancing Indirect Interaction");
    }

    private void Update()
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _pusher.Translate(dir * (_pusherSpeed * Time.deltaTime));
        
        _compute.SetVector("pusher_position", _pusher.position);
        _compute.Dispatch(_kernel, Mathf.CeilToInt(_count / 64f), 1, 1);

        Graphics.DrawMeshInstancedIndirect(_instanceMesh, 0, _instanceMaterial, new Bounds(Vector3.zero, Vector3.one * 1000), _argsBuffer);
        
        if (Input.GetMouseButtonUp(0) && _countMultiplier != _cachedMultiplier)
        {
            _countMultiplier = _cachedMultiplier;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDisable()
    {
        _argsBuffer?.Release();
        _argsBuffer = null;
        
        _meshPropertiesBuffer?.Release();
        _meshPropertiesBuffer = null;
    }

    private void UpdateBuffers()
    {
        var offset = Vector3.zero;
        var data = new MeshData[_count];
        for (var i = 0; i < _count; i++)
        {
            var pos = Random.insideUnitSphere.normalized * Random.Range(10, 50) + offset;
            var rot = Quaternion.Euler(Random.insideUnitSphere.normalized);

            data[i] = new MeshData
            {
                BasePos = pos,
                Mat = Matrix4x4.TRS(pos, rot, SceneTools.CubeScale),
                Amount = 0
            };
        }

        _meshPropertiesBuffer = new ComputeBuffer(_count, 80);
        _meshPropertiesBuffer.SetData(data);

        _compute.SetBuffer(_kernel, "data", _meshPropertiesBuffer);
        _instanceMaterial.SetBuffer("data", _meshPropertiesBuffer);

        // Verts
        _args[0] = _instanceMesh.GetIndexCount(0);
        _args[1] = (uint)_count;
        _args[2] = _instanceMesh.GetIndexStart(0);
        _args[3] = _instanceMesh.GetBaseVertex(0);

        _argsBuffer.SetData(_args);
    }
    
    public void UpdateMultiplier(float val)
    {
        ApplyMultiplierUpdate(Mathf.CeilToInt(val));
    }

    private void ApplyMultiplierUpdate(int val, bool applySliderChange = false)
    {
        _sliderValueText.text = $"Multiplier: {val.ToString()}";
        _cachedMultiplier = val;
        if(applySliderChange) _slider.value = val;
    }

    private struct MeshData
    {
        public float3 BasePos;
        public Matrix4x4 Mat;
        public float Amount;
    }
}