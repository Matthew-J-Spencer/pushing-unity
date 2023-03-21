using UnityEngine;

public class Level1 : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;

    private void Start()
    {
        SceneTools.LoopPositions((i, p) =>
        {
            Instantiate(_cubePrefab, new Vector3(p.x, p.y * SceneTools.DEPTH_OFFSET, p.z), Quaternion.identity, transform)
                .AddComponent<Level1Cube>()
                .Init(p.y);
        });
        
        SceneTools.Instance.SetCountText(SceneTools.GetCount);
        SceneTools.Instance.SetNameText("Individual MonoBehaviours");
    }
}