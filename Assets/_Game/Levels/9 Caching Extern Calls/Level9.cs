using System;
using UnityEngine;

public class Level9 : MonoBehaviour
{
    [SerializeField] private int _iterations;
    [SerializeField] private TestMode _mode;

    private int _fixedFrame;
    private float TestProperty => _fixedFrame;
    
    private void Start()
    {
        SceneTools.Instance.SetNameText("Caching... any extern");
        SceneTools.Instance.SetCountText(_iterations);
    }

    private void FixedUpdate()
    {
        _fixedFrame++;
    }

    private void Update()
    {
        switch (_mode)
        {
            case TestMode.Extern:
                for (var i = 0; i < _iterations; i++)
                {
                    var result = Time.time;
                }
                break;
            case TestMode.Cache:
                var time = Time.time;
                for (var i = 0; i < _iterations; i++)
                {
                    var result = time;
                }
                break;
            case TestMode.Property:
                for (var i = 0; i < _iterations; i++)
                {
                    var result = TestProperty;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ChangeMode(int mode)
    {
        _mode = (TestMode)mode;
    }

    private enum TestMode
    {
        Extern = 0,
        Cache = 1,
        Property = 2
    }
}
