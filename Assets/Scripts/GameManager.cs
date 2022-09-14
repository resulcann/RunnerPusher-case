using System;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum OperationState
{
    None,
    Addition,
    Substraction,
    Multiplication,
    Division
}
public enum UnitColor
{
    Red,
    Blue,
    Yellow
}

public enum GameState
{
    None,
    Loading,
    Gameplay,
    Win,
    Lose,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Material addMat, subMat, multMat, divMat;
    public TextMeshProUGUI currentCrowdNumber;
    public Button tapToStartBtn;
    public SplineFollower splineFollower;
    public GameState gameState;
    public float crowdSpeed;


    private void Awake()
    {
        Instance = this;
        tapToStartBtn.gameObject.SetActive(true);
        gameState = GameState.Loading;
        splineFollower.follow = false;
    }
    
    private void Update()
    {
        if (gameState != GameState.Gameplay) return;
        splineFollower.followSpeed = crowdSpeed;
    }

    public void StartGamePlay()
    {
        gameState = GameState.Gameplay;
        splineFollower.follow = true;
        tapToStartBtn.gameObject.SetActive(false);
    }
    
    public void WinGame()
    {
        
    }

    public void LoseGame()
    {
        
    }
    
}
