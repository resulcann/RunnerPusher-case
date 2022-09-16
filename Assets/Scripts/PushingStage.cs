using System.Linq;
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
    public Colors leftStageColor;

    [Header("MiddleStage References")] [Space(10)]
    [SerializeField] private TextMeshProUGUI middleStageText;
    public Transform middleStageCharacters;
    private int _middleStageCharacterCount;
    public Unit[] middleCharacters;
    public Colors middleStageColor;
    
    [Header("RightStage References")] [Space(10)]
    [SerializeField] private TextMeshProUGUI rightStageText;
    public Transform rightStageCharacters;
    private int _rightStageCharacterCount;
    public Unit[] rightCharacters;
    public Colors rightStageColor;

    private void Awake()
    {
        Instance = this;
        
        leftCharacters = leftStageCharacters.GetComponentsInChildren<Unit>();
        middleCharacters = middleStageCharacters.GetComponentsInChildren<Unit>();
        rightCharacters = rightStageCharacters.GetComponentsInChildren<Unit>();
        
        _leftStageCharacterCount = leftCharacters.Length;
        _middleStageCharacterCount = middleCharacters.Length;
        _rightStageCharacterCount = rightCharacters.Length;

        leftStageText.text = _leftStageCharacterCount.ToString();
        middleStageText.text = _middleStageCharacterCount.ToString();
        rightStageText.text = _rightStageCharacterCount.ToString();

        leftStageColor = leftCharacters.First().stickManColor;
        middleStageColor = middleCharacters.First().stickManColor;
        rightStageColor = rightCharacters.First().stickManColor;
    }
    
}
