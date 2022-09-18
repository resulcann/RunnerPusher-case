using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class CrowdCollisions : MonoBehaviour
{
    public static CrowdCollisions Instance { get; private set; }
    private static readonly int Pushing = Animator.StringToHash("Pushing");
    private static readonly int Run = Animator.StringToHash("Run");
    private Transform _pusherMans;
    private bool _isPushingBack, _goToCrew;
    public List<Unit> pusherMans = new List<Unit>();
    private int _stairsHeight;
    private static readonly int Win = Animator.StringToHash("Win");
    private static readonly int Sitting = Animator.StringToHash("Sitting");

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_isPushingBack) _pusherMans.Translate(Vector3.forward * (Time.deltaTime * GameManager.Instance.crowdSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherGo = other.gameObject;
        
        
        if (otherGo.CompareTag("Collectible"))
        {
            var mathOperation = otherGo.GetComponent<MathOperations>();
            switch (otherGo.GetComponent<MathOperations>().operationState)
            {
                case OperationState.Addition:
                    FormationController.Instance.numberOfUnit += mathOperation.number;
                    Destroy(otherGo);
                    break;
                case OperationState.Substraction:
                    FormationController.Instance.numberOfUnit -= mathOperation.number;
                    Destroy(otherGo);
                    break;
                case OperationState.Multiplication:
                    FormationController.Instance.numberOfUnit *= mathOperation.number;
                    Destroy(otherGo);
                    break;
                case OperationState.Division:
                    FormationController.Instance.numberOfUnit /= mathOperation.number;
                    Destroy(otherGo);
                    break;
                default:
                    Destroy(otherGo);
                    break;
            }

            if (FormationController.Instance.numberOfUnit <= 0)
            {
                GameManager.Instance.FinishGamePlay(false);
            }
        }

        if (otherGo.CompareTag("PushingStage") && otherGo.GetComponentInChildren<Unit>().stickManColor != GameManager.Instance.unitColor)
        {
            var pushingStage = otherGo.GetComponentInParent<PushingStage>();
            
            foreach (var unit in GetComponent<UnitController>().SpawnedUnits)
            {
                unit.GetComponent<Animator>().SetBool(Pushing,true);
                unit.GetComponent<Animator>().SetBool(Run,false);
            }
            if (otherGo == pushingStage.leftStageCharacters.gameObject)
            {
                foreach (var unit in pushingStage.leftCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Pushing,true);
                    unit.GetComponent<Animator>().SetBool(Run,false);
                }
                _pusherMans = pushingStage.leftStageCharacters;
                pushingStage.middleStageCharacters.GetComponent<BoxCollider>().enabled = false;
                pushingStage.rightStageCharacters.GetComponent<BoxCollider>().enabled = false;
            }
            else if (otherGo == pushingStage.middleStageCharacters.gameObject)
            {
                foreach (var unit in pushingStage.middleCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Pushing,true);
                    unit.GetComponent<Animator>().SetBool(Run,false);
                }
                _pusherMans = pushingStage.middleStageCharacters;
                pushingStage.leftStageCharacters.GetComponent<BoxCollider>().enabled = false;
                pushingStage.rightStageCharacters.GetComponent<BoxCollider>().enabled = false;
            }
            else if (otherGo == pushingStage.rightStageCharacters.gameObject)
            {
                foreach (var unit in pushingStage.rightCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Pushing,true);
                    unit.GetComponent<Animator>().SetBool(Run,false);
                }
                _pusherMans = pushingStage.rightStageCharacters;
                pushingStage.middleStageCharacters.GetComponent<BoxCollider>().enabled = false;
                pushingStage.leftStageCharacters.GetComponent<BoxCollider>().enabled = false;
            }

            transform.DOMoveX(otherGo.transform.position.x -0.5f, 0.1f);
            GameManager.Instance.CanSwipe = false;

            if (FormationController.Instance.numberOfUnit >= otherGo.transform.childCount)
            {
                StartCoroutine(Push(otherGo.transform));
            }
            else if (FormationController.Instance.numberOfUnit < otherGo.transform.childCount)
            {
                StartCoroutine(PushedBack(otherGo.transform));
            }
        }
        
        if (otherGo.CompareTag("PushingMan") && otherGo.GetComponentInChildren<Unit>().stickManColor == UnitController.Instance.SpawnedUnits[0].stickManColor)
        {
            pusherMans.Remove(otherGo.GetComponent<Unit>());
            Destroy(otherGo);
            FormationController.Instance.numberOfUnit++;
        }
        if(otherGo.CompareTag("FinishLine"))
        {
            otherGo.GetComponent<Animation>().Play();
            StartCoroutine(UnitController.Instance.TowerFormation());
        }

        if (otherGo.CompareTag("FinishStairs"))
        {
            var allUnits = UnitController.Instance.SpawnedUnits;
            if (allUnits.Count > 3)
            {
                transform.parent.position += Vector3.up;
                for (var i = 0; i < 3; i++)
                {
                    var lastUnit = allUnits.Last();
                    var lastUnitTransform = lastUnit.transform;
                    var lastUnitPos = lastUnitTransform.position;
                    lastUnit.GetComponent<Animator>().SetBool(Win,true);
                    lastUnit.GetComponent<Animator>().SetBool(Sitting,false);
                    allUnits.Remove(lastUnit);
                    FormationController.Instance.numberOfUnit--;
                    lastUnit.transform.parent = transform.root;
                    lastUnitTransform.position = new Vector3(lastUnitPos.x, _stairsHeight, lastUnitPos.z -0.75f);
                }
                GameManager.Instance.mainCam.GetComponent<CameraFollower>().SetFinishCameraPos(-1f);
                _stairsHeight++;
            }
            else 
                GameManager.Instance.FinishGamePlay(true);
            
        }
        
        
    }
    
    private IEnumerator Push(Component otherStageCharacters)
    {
        pusherMans = otherStageCharacters.GetComponentsInChildren<Unit>().ToList();
        GameManager.Instance.crowdSpeed = 0f;
        yield return new WaitForSeconds(.5f);
        GameManager.Instance.crowdSpeed = 1.5f;
        _isPushingBack = true;
        yield return new WaitUntil( ()=> pusherMans.Count == 0);
        _isPushingBack = false;
        GameManager.Instance.crowdSpeed = GameManager.Instance.tempSpeed;
        GameManager.Instance.CanSwipe = true;
        foreach (var unit in GetComponent<UnitController>().SpawnedUnits)
        {
            unit.GetComponent<Animator>().SetBool(Pushing,false);
            unit.GetComponent<Animator>().SetBool(Run,true);
        }
    }
    
    private IEnumerator PushedBack(Component otherStageCharacters)
    {
        pusherMans = otherStageCharacters.GetComponentsInChildren<Unit>().ToList();
        GameManager.Instance.crowdSpeed = 0f;
        yield return new WaitForSeconds(.5f);
        GameManager.Instance.crowdSpeed = -1.5f;
        _isPushingBack = true;
        GameManager.Instance.mainCam.GetComponent<CameraFollower>().enabled = false;
        yield return new WaitUntil(() =>
        {
            Camera main;
            return (main = Camera.main) != null && transform.position.z < main.transform.position.z + 2f;
        });
        _isPushingBack = false;
        GameManager.Instance.FinishGamePlay(false);
    }
}
