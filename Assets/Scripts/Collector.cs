using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Collector : MonoBehaviour
{
    public static Collector Instance { get; private set; }
    private static readonly int Pushing = Animator.StringToHash("Pushing");
    private static readonly int Run = Animator.StringToHash("Run");
    private Transform _pusherMans;
    private bool _isPushingBack, _goToCrew;
    public List<Unit> pusherMans = new List<Unit>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_isPushingBack) _pusherMans.Translate(Vector3.forward * (Time.deltaTime * GameManager.Instance.crowdSpeed));
        if (!_goToCrew) return;
        _pusherMans.position = Vector3.MoveTowards(_pusherMans.position,
            UnitController.Instance.SpawnedUnits.First().transform.position,
            GameManager.Instance.crowdSpeed * 3f * Time.deltaTime);
    // Todo go to crew düzgün çalısmıyor!!

    }

    private void OnTriggerEnter(Collider other)
    {
        var otherGo = other.gameObject;
        
        
        if (otherGo.CompareTag("Collectible"))
        {
            switch (otherGo.GetComponent<Collectible>().operationState)
            {
                case OperationState.Addition:
                    FormationController.Instance.numberOfUnit += otherGo.GetComponent<Collectible>().number;
                    Destroy(otherGo);
                    break;
                case OperationState.Substraction:
                    FormationController.Instance.numberOfUnit -= otherGo.GetComponent<Collectible>().number;
                    Destroy(otherGo);
                    break;
                case OperationState.Multiplication:
                    FormationController.Instance.numberOfUnit *= otherGo.GetComponent<Collectible>().number;
                    Destroy(otherGo);
                    break;
                case OperationState.Division:
                    FormationController.Instance.numberOfUnit /= otherGo.GetComponent<Collectible>().number;
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
                if (pushingStage.leftStageColor == GameManager.Instance.unitColor) _goToCrew = true;
            }
            else if (otherGo == pushingStage.middleStageCharacters.gameObject)
            {
                foreach (var unit in pushingStage.middleCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Pushing,true);
                    unit.GetComponent<Animator>().SetBool(Run,false);
                }
                _pusherMans = pushingStage.middleStageCharacters;
                if (pushingStage.middleStageColor == GameManager.Instance.unitColor)
                {
                    _goToCrew = true;
                    Debug.Log("midden geçti");
                }
            }
            else if (otherGo == pushingStage.rightStageCharacters.gameObject)
            {
                foreach (var unit in pushingStage.rightCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Pushing,true);
                    unit.GetComponent<Animator>().SetBool(Run,false);
                }
                _pusherMans = pushingStage.rightStageCharacters;
                if (pushingStage.rightStageColor == GameManager.Instance.unitColor) _goToCrew = true;
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
        else if (otherGo.CompareTag("PushingStage") && otherGo.GetComponentInChildren<Unit>().stickManColor == GameManager.Instance.unitColor)
        {
            var pushingStage = otherGo.GetComponentInParent<PushingStage>();
            
            if (otherGo == pushingStage.leftStageCharacters.gameObject)
            {
                foreach (var unit in pushingStage.leftCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Run,true);
                }
                _pusherMans = pushingStage.leftStageCharacters;
            }
            else if (otherGo == pushingStage.middleStageCharacters.gameObject)
            {
                foreach (var unit in pushingStage.middleCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Run,true);
                }
                _pusherMans = pushingStage.middleStageCharacters;
            }
            else if (otherGo == pushingStage.rightStageCharacters.gameObject)
            {
                foreach (var unit in pushingStage.rightCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Run,true);
                }
                _pusherMans = pushingStage.rightStageCharacters;
            }
            _goToCrew = true;
        }
        if (otherGo.CompareTag("PushingMan") && otherGo.GetComponentInChildren<Unit>().stickManColor == UnitController.Instance.SpawnedUnits[0].stickManColor)
        {
            pusherMans.Remove(otherGo.GetComponent<Unit>());
            Destroy(otherGo);
            FormationController.Instance.numberOfUnit++;
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
        if (Camera.main != null) Camera.main.GetComponent<CameraFollower>().enabled = false;
        yield return new WaitUntil(() =>
        {
            Camera main;
            return (main = Camera.main) != null && transform.position.z < main.transform.position.z + 2f;
        });
        _isPushingBack = false;
        GameManager.Instance.FinishGamePlay(false);
    }
}
