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
public enum Colors
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
    public bool IsActive { get; set; }
    public Material addMat, subMat, multMat, divMat;
    public Material redMat, blueMat, yellowMat;
    public TextMeshProUGUI currentCrowdNumber;
    public Image crowdNumberImg;
    public Button tapToStartBtn;
    [HideInInspector] public GameState gameState;
    public float crowdSpeed, tempSpeed;
    [HideInInspector] public int pushingManCount;
    private static readonly int Run = Animator.StringToHash("Run");


    private void Awake()
    {
        Instance = this;
        tapToStartBtn.gameObject.SetActive(true);
        gameState = GameState.Loading;
        tempSpeed = crowdSpeed;
        crowdSpeed = 0f;
    }
    
    public void StartGamePlay()
    {
        gameState = GameState.Gameplay;
        tapToStartBtn.gameObject.SetActive(false);
        crowdSpeed = tempSpeed;
        IsActive = true;

        foreach (var unit in UnitController.Instance.SpawnedUnits)
        {
            unit.GetComponent<Animator>().SetBool(Run,true);
        }
        
    }
}