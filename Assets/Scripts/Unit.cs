using System.Linq;
using UnityEngine;
public class Unit : MonoBehaviour
{
    public Colors stickManColor = Colors.Yellow;
    public new Renderer renderer;
    
    private void Start()
    {
        if (stickManColor == Colors.Blue) renderer.material = GameManager.Instance.blueMat;
        else if (stickManColor == Colors.Red) renderer.material = GameManager.Instance.redMat;
        else if (stickManColor == Colors.Yellow) renderer.material = GameManager.Instance.yellowMat;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var otherGo = other.gameObject;
        var units = UnitController.Instance.SpawnedUnits;
        var unitCount = UnitController.Instance.SpawnedUnits.Count;
        
        if (otherGo.CompareTag("Cutter"))
        {
            var leaverCount = unitCount - (units.IndexOf(this) + 1);
            FormationController.Instance.numberOfUnit -= leaverCount;   //Engelin çarptığı unit'den öncesini siler
            if (FormationController.Instance.numberOfUnit <= 0) GameManager.Instance.FinishGamePlay(false);
        }
        else if (otherGo.CompareTag("Enemy"))
        {
            FormationController.Instance.numberOfUnit--;  // Sadece engelin çarptığı unit'i siler
            if(FormationController.Instance.numberOfUnit <= 0 ) GameManager.Instance.FinishGamePlay(false);
        }
        
    }
}
