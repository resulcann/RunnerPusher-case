using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ColorGate : MonoBehaviour
{
    [SerializeField] private Colors gateColor;
    [SerializeField] private Material gateMaterial;

    private void OnTriggerEnter(Collider other)
    {
        var otherGo = other.gameObject;

        if (otherGo.CompareTag("Stickman") && this.gateColor != otherGo.GetComponent<Unit>().stickManColor)
        {
            otherGo.GetComponent<Unit>().stickManColor = gateColor;
            otherGo.GetComponent<Unit>().renderer.material = gateMaterial;
            GameManager.Instance.crowdNumberImg.GetComponent<Image>().color = gateColor switch
            {
                Colors.Blue => new Color32(100, 225, 255, 255),
                Colors.Red => new Color32(255, 101, 116, 255),
                Colors.Yellow => new Color32(240, 230, 130, 255),
                _ => new Color32(240, 230, 130, 255)
            };
        }
        else if (otherGo.CompareTag("PushingMan") && this.gateColor != otherGo.GetComponent<Unit>().stickManColor)
        {
            otherGo.GetComponent<Unit>().stickManColor = gateColor;
            otherGo.GetComponent<Unit>().renderer.material = gateMaterial;
            otherGo.transform.DORotate(new Vector3(0f,-360f,0f), 1f);
            otherGo.transform.DOMove(UnitController.Instance.SpawnedUnits.Last().transform.position, .2f).OnComplete(()=>
                Collector.Instance.pusherMans.RemoveAt(Collector.Instance.pusherMans.Count-1));
            DOTween.CompleteAll();
            Destroy(otherGo);
            FormationController.Instance.numberOfUnit++;
        }
    }
}
