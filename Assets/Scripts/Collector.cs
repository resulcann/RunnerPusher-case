using UnityEngine;

public class Collector : MonoBehaviour
{
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
    }
}
