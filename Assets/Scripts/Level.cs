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
        GameManager.Instance.unitColor = Colors.Yellow;
    }

    public void DestroyLevel()
    {
        DestroyImmediate(_spawnedLevelPrefab);
    }
}
