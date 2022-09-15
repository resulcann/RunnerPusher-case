using UnityEngine;

public class SwipeMovement : MonoBehaviour
{


    [Header("Movement")]
    [SerializeField] private float dragMultiplier = 2f;
    [SerializeField] private Vector2 bounds;

    private float _mouseStartPos;
    private float _mouseCurrentPos;
    private float _playerCurrentPos;


    private void Start()
    {
        _playerCurrentPos = transform.localPosition.x;
        _mouseStartPos = Input.mousePosition.x;
        _mouseStartPos /= Screen.width;
    }


    void Update()
    {
        if (!GameManager.Instance.IsActive) return;
        transform.parent.Translate(Vector3.forward * Time.deltaTime * GameManager.Instance.crowdSpeed);
        HandLeMovement();
    }

    void HandLeMovement()
    {

        if (Input.GetMouseButtonDown(0))
        {
            _playerCurrentPos = transform.localPosition.x;
            _mouseStartPos = Input.mousePosition.x;
            _mouseStartPos /= Screen.width;

        }
        if (Input.GetMouseButton(0))
        {
            var localPos = transform.localPosition;
            _mouseCurrentPos = Input.mousePosition.x;
            _mouseCurrentPos /= Screen.width;
            float targetPos = _playerCurrentPos + (_mouseCurrentPos - _mouseStartPos) * dragMultiplier;
            targetPos = Mathf.Clamp(targetPos, bounds.x, bounds.y);

            transform.localPosition = new Vector3(targetPos, localPos.y, localPos.z);
        }

    }




}