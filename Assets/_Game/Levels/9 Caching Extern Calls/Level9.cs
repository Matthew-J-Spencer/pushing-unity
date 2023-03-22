using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Level9 : MonoBehaviour
{
    [SerializeField] private int _iterations;
    [SerializeField] private TestMode _mode;
    [SerializeField] private TMP_Text _modeText;

    private Camera CamProperty
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        get;
        set;
    }

    private void Start()
    {
        CamProperty = Camera.main;
        ChangeMode(0);
        SceneTools.Instance.SetNameText("Caching Camera.main");
        SceneTools.Instance.SetCountText(_iterations);
    }

    private void Update()
    {
        switch (_mode)
        {
            case TestMode.Extern:
                for (var i = 0; i < _iterations; i++)
                {
                    var result = Camera.main;
                }
                break;
            case TestMode.Cache:
                var cam = Camera.main; 
                for (var i = 0; i < _iterations; i++)
                {
                    var result = cam;
                }
                break;
            case TestMode.Property:
                for (var i = 0; i < _iterations; i++)
                {
                    var result = CamProperty;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ChangeMode(int mode)
    {
        _mode = (TestMode)mode;
        _modeText.text = $"Mode: {_mode.ToString()}";
    }

    private enum TestMode
    {
        Extern = 0,
        Cache = 1,
        Property = 2,
    }
}
