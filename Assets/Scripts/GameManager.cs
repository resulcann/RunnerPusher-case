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
    public bool CanSwipe { get; set; }
    public Material addMat, subMat, multMat, divMat;
    public Material redMat, blueMat, yellowMat;
    [SerializeField] private Transform finishSuccessUI, finishFailUI;
    public Button tapToStartBtn;
    [HideInInspector] public GameState gameState;
    public float crowdSpeed, tempSpeed;
    public Colors unitColor;
    private static readonly int Run = Animator.StringToHash("Run");


    private void Awake()
    {
        Instance = this;
        tapToStartBtn.gameObject.SetActive(true);
        finishSuccessUI.gameObject.SetActive(false);
        finishFailUI.gameObject.SetActive(false);
        gameState = GameState.Loading;
        tempSpeed = crowdSpeed;
        crowdSpeed = 0f;
        unitColor = Colors.Yellow;
    }
    
    public void StartGamePlay()
    {
        gameState = GameState.Gameplay;
        tapToStartBtn.gameObject.SetActive(false);
        finishSuccessUI.gameObject.SetActive(false);
        finishFailUI.gameObject.SetActive(false);
        if (Camera.main != null) Camera.main.GetComponent<CameraFollower>().enabled = true;
        crowdSpeed = tempSpeed;
        IsActive = true;
        CanSwipe = true;
        
        foreach (var unit in UnitController.Instance.SpawnedUnits)
        {
            unit.GetComponent<Animator>().SetBool(Run,true);
        }
        
    }

    public void FinishGamePlay(bool success)
    {
        IsActive = false;
        if(success) finishSuccessUI.gameObject.SetActive(true);
        else finishFailUI.gameObject.SetActive(true);
    }
}