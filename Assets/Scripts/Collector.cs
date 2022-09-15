using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public static Collector Instance { get; private set; }
    private static readonly int Pushing = Animator.StringToHash("Pushing");
    private static readonly int Run = Animator.StringToHash("Run");
    private Transform _pusherMans;
    private bool _canMove = false;
    public List<Unit> pusherMans = new List<Unit>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!_canMove) return;
        _pusherMans.Translate(Vector3.forward * (Time.deltaTime * GameManager.Instance.crowdSpeed));
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
        }

        if (otherGo.CompareTag("PushingStage"))
        {
            foreach (var unit in GetComponent<UnitController>().SpawnedUnits)
            {
                unit.GetComponent<Animator>().SetBool(Pushing,true);
                unit.GetComponent<Animator>().SetBool(Run,false);
            }

            if (otherGo == PushingStage.Instance.leftStageCharacters.gameObject)
            {
                foreach (var unit in PushingStage.Instance.leftCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Pushing,true);
                    unit.GetComponent<Animator>().SetBool(Run,false);
                }
                _pusherMans = PushingStage.Instance.leftStageCharacters;
            }
            else if (otherGo == PushingStage.Instance.middleStageCharacters.gameObject)
            {
                foreach (var unit in PushingStage.Instance.middleCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Pushing,true);
                    unit.GetComponent<Animator>().SetBool(Run,false);
                }
                _pusherMans = PushingStage.Instance.middleStageCharacters;
            }
            else if (otherGo == PushingStage.Instance.rightStageCharacters.gameObject)
            {
                foreach (var unit in PushingStage.Instance.rightCharacters)
                {
                    unit.GetComponent<Animator>().SetBool(Pushing,true);
                    unit.GetComponent<Animator>().SetBool(Run,false);
                }
                _pusherMans = PushingStage.Instance.rightStageCharacters;
            }

            if (FormationController.Instance.numberOfUnit >= otherGo.transform.childCount)
            {
                StartCoroutine(Push(otherGo.transform));
            }

        }
    }
    
    private IEnumerator Push(Transform otherStageCharacters)
    {
        pusherMans = otherStageCharacters.GetComponentsInChildren<Unit>().ToList();
        GameManager.Instance.crowdSpeed = 0f;
        yield return new WaitForSeconds(.5f);
        GameManager.Instance.crowdSpeed = 1.5f;
        _canMove = true;
        yield return new WaitUntil( ()=> pusherMans.Count == 0);
        _canMove = false;
        GameManager.Instance.crowdSpeed = GameManager.Instance.tempSpeed;
        foreach (var unit in GetComponent<UnitController>().SpawnedUnits)
        {
            unit.GetComponent<Animator>().SetBool(Pushing,false);
            unit.GetComponent<Animator>().SetBool(Run,true);
        }


        // stage karakterleri itmiyor onu hallet , ittikten sonra itilenleri ekibe kat, çıkışta hızı ve animasyonu tekrar ayarla!
    }
}
