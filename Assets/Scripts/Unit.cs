using UnityEngine;
public class Unit : MonoBehaviour
{
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
