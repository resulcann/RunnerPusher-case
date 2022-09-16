using UnityEngine;

[CreateAssetMenu(fileName ="New Level", menuName ="Levels", order = 1)]
public class Level : ScriptableObject
{
    public GameObject levelPrefab;
    public int levelIndex;
    private GameObject _spawnedLevelPrefab;

    public void CreateLevel()
    {
        _spawnedLevelPrefab = Instantiate(levelPrefab);
        if (Camera.main == null) return;
        var mainCam = Camera.main.GetComponent<CameraFollower>();
        mainCam.target = FindObjectOfType<UnitController>().transform.parent;
        mainCam.transform.position = new Vector3(0f,0f,mainCam.offset.z);
        GameManager.Instance.unitColor = Colors.Yellow;

    }

    public void DestroyLevel()
    {
        DestroyImmediate(_spawnedLevelPrefab);
    }
}
