using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Level> levels;
    public int currentLevelIndex;
    [HideInInspector] public bool isRandom;
    private CameraFollower _cameraFollower;

    private void Awake()
    {
        isRandom = false;
        _cameraFollower = GameManager.Instance.mainCam.GetComponent<CameraFollower>();
        if (currentLevelIndex == levels.Count-1) isRandom = true;
        
    }

    private void Start()
    {
        SpawnCurrentLevel();
    }

    private void SpawnCurrentLevel()
    {
        levels[currentLevelIndex].CreateLevel();
        _cameraFollower.SetDefaultCameraValues();
    }

    public void NextLevel()
    {
        if (isRandom == false)
        {
            levels[currentLevelIndex].DestroyLevel();
            currentLevelIndex++;
            levels[currentLevelIndex].CreateLevel();
            if (currentLevelIndex == levels.Count-1) isRandom = true;
        }
        else
        {
            var randomValue = Random.Range(0, levels.Count);
            levels[currentLevelIndex].DestroyLevel();
            while (randomValue == currentLevelIndex) randomValue = Random.Range(0, levels.Count);
            currentLevelIndex = randomValue;
            levels[currentLevelIndex].CreateLevel();
        }
        _cameraFollower.SetDefaultCameraValues();
        GameManager.Instance.StartGamePlay();

    }
    public void RetryLevel()
    {
        levels[currentLevelIndex].DestroyLevel();
        levels[currentLevelIndex].CreateLevel();
        _cameraFollower.SetDefaultCameraValues();
        GameManager.Instance.StartGamePlay();
    }

}