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
            PaintStickMan(otherGo);
        }
        else if (otherGo.CompareTag("PushingMan"))
        {
            PaintPushingMan(otherGo);
        }
    }

    private void PaintStickMan(GameObject stickMan)
    {
        stickMan.GetComponent<Unit>().stickManColor = gateColor;
        stickMan.GetComponent<Unit>().renderer.material = gateMaterial;
        switch (gateColor)
        {
            case Colors.Blue:
                UnitController.Instance.crowdNumberImg.GetComponent<Image>().color =
                    new Color32(100, 225, 255, 255);
                GameManager.Instance.unitColor = Colors.Blue;
                break;
            case Colors.Red:
                UnitController.Instance.crowdNumberImg.GetComponent<Image>().color =
                    new Color32(255, 101, 116, 255);
                GameManager.Instance.unitColor = Colors.Red;
                break;
            case Colors.Yellow:
            default:
                UnitController.Instance.crowdNumberImg.GetComponent<Image>().color =
                    new Color32(240, 230, 130, 255);
                GameManager.Instance.unitColor = Colors.Yellow;
                break;
        }
    }

    private void PaintPushingMan(GameObject pushingMan)
    {
        pushingMan.GetComponent<Unit>().stickManColor = gateColor;
        pushingMan.GetComponent<Unit>().renderer.material = gateMaterial;
        pushingMan.transform.DORotate(new Vector3(0f,-360f,0f), 1f);
        pushingMan.transform.DOMove(UnitController.Instance.SpawnedUnits.Last().transform.position, .2f).OnComplete(()=>
            CrowdCollisions.Instance.pusherMans.RemoveAt(CrowdCollisions.Instance.pusherMans.Count-1));
        DOTween.CompleteAll();
        Destroy(pushingMan);
        FormationController.Instance.numberOfUnit++;
    }
}
