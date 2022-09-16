using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.25f;
    public Vector3 offset;
    [SerializeField] private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        var camPos = transform.position;
        var desiredPos = target.position + offset;
        transform.position = Vector3.SmoothDamp(camPos, desiredPos, ref velocity, smoothTime);
    }
}