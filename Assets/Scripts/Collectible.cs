using TMPro;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    public OperationState operationState;
    public int number;


    private void Start()
    {
        switch (operationState)
        {
            case OperationState.Addition:
                numberText.text = "+" + number;
                GetComponent<Renderer>().material = GameManager.Instance.addMat;
                break;
            case OperationState.Substraction:
                numberText.text = "-" + number;
                GetComponent<Renderer>().material = GameManager.Instance.subMat;
                break;
            case OperationState.Multiplication:
                numberText.text = "x" + number;
                GetComponent<Renderer>().material = GameManager.Instance.multMat;
                break;
            case OperationState.Division:
                numberText.text = "÷" + number;
                GetComponent<Renderer>().material = GameManager.Instance.divMat;
                break;
        }
    }
}
