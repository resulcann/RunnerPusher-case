using TMPro;
using UnityEngine;

public class PushingStage : MonoBehaviour
{
    public static PushingStage Instance { get; private set; }
    
    [Header("LeftStage References")] [Space(10)]
    [SerializeField] private TextMeshProUGUI leftStageText;
    public Transform leftStageCharacters;
    private int _leftStageCharacterCount;
    public Unit[] leftCharacters;

    [Header("MiddleStage References")] [Space(10)]
    [SerializeField] private TextMeshProUGUI middleStageText;
    public Transform middleStageCharacters;
    private int _middleStageCharacterCount;
    public Unit[] middleCharacters;
    
    [Header("RightStage References")] [Space(10)]
    [SerializeField] private TextMeshProUGUI rightStageText;
    public Transform rightStageCharacters;
    private int _rightStageCharacterCount;
    public Unit[] rightCharacters;

    private void Awake()
    {
        Instance = this;
        
        _leftStageCharacterCount = leftStageCharacters.childCount;
        _middleStageCharacterCount = middleStageCharacters.childCount;
        _rightStageCharacterCount = rightStageCharacters.childCount;

        leftStageText.text = _leftStageCharacterCount.ToString();
        middleStageText.text = _middleStageCharacterCount.ToString();
        rightStageText.text = _rightStageCharacterCount.ToString();

        leftCharacters = leftStageCharacters.GetComponentsInChildren<Unit>();
        middleCharacters = middleStageCharacters.GetComponentsInChildren<Unit>();
        rightCharacters = rightStageCharacters.GetComponentsInChildren<Unit>();
    }
    
}
