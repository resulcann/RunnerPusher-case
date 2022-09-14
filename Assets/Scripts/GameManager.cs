using UnityEngine;

public enum OperationState
{
    None,
    Addition,
    Substraction,
    Multiplication,
    Division
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Material addMat, subMat, multMat, divMat;

    private void Awake()
    {
        Instance = this;
    }
}
