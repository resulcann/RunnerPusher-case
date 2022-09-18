using DG.Tweening;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private Vector3 offset;
    private Vector3 _defaultOffset, _defaultRotation;
    public bool isFollow;


    private void Awake()
    {
        _defaultOffset = offset;
        _defaultRotation = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        //if (!isFollow) return;
        var camPos = transform.position;
        var desiredPos = target.position + offset;
        transform.position = Vector3.SmoothDamp(camPos, desiredPos, ref velocity, smoothTime);
    }

    public void SetDefaultCameraValues()
    {
        DOTween.KillAll();
        target = FindObjectOfType<UnitController>().transform.parent;
        offset = _defaultOffset;
        transform.eulerAngles = _defaultRotation;
        transform.position = offset;
    }

    public void SetFinishCameraPos(float posYamount)
    {
        offset.y += posYamount;
        offset.x = (offset.y / 4) + 1;
        offset.z = -(offset.y / 2) - 2;
        transform.DORotate(new Vector3(30f, -20f, 0f), 2f);
    }
}