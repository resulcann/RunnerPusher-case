using UnityEngine;
public class Unit : MonoBehaviour
{
    private Animator _animator;
    public Colors stickManColor = Colors.Yellow;
    public new Renderer renderer;
    private static readonly int Run = Animator.StringToHash("Run");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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
        
        if (otherGo.CompareTag("Enemy"))
        {
            var leaverCount = unitCount - units.IndexOf(this);
            FormationController.Instance.numberOfUnit -= leaverCount;
        }
        
    }
}
