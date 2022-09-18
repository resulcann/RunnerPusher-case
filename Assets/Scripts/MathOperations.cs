using TMPro;
using UnityEngine;

public class MathOperations : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    public OperationState operationState;
    public int number;
    public float moveSpeed;
    public bool canPingPong;


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

        moveSpeed = Random.Range(moveSpeed / 1.5f, moveSpeed * 1.5f);
    }

    private void Update()
    {
        if (!canPingPong) return;
        var pos = transform.position;
        transform.position = new Vector3(Mathf.PingPong( moveSpeed * Time.time, 6f) -3, pos.y, pos.z);
    }
}
