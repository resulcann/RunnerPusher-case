using System;
using UnityEngine;
public class Unit : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Run = Animator.StringToHash("Run");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Gameplay) return;
        _animator.SetBool(Run,true);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherGo = other.gameObject;
        var units = UnitController.Instance.SpawnedUnits;
        var unitCount = UnitController.Instance.SpawnedUnits.Count;
        
        if (otherGo.CompareTag("Enemy"))
        {
            var leaverCount = unitCount - units.IndexOf(this);
            FormationController.Instance.numberOfUnit -= leaverCount;
        }
        
    }
}
