using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public static UnitController Instance { get; private set; }
    private FormationController _formation;
    public FormationController Formation {
        get {
            if (_formation == null) _formation = GetComponent<FormationController>();
            return _formation;
        }
        set => _formation = value;
    }

    [SerializeField] private Unit unitPrefab;
    [SerializeField] private float snakeMoveSpeed = 20;


    public readonly List<Unit> SpawnedUnits = new List<Unit>();
    private List<Vector3> _points = new List<Vector3>();
    private Transform _units;

    private void Awake()
    {
        Instance = this;
        _units = new GameObject("Units").transform;
        _units.parent = transform.parent;
    }

    private void Update() 
    {
        SetFormation();
    }

    private void SetFormation() {
        _points = Formation.EvaluatePoints().ToList();

        if (_points.Count > SpawnedUnits.Count) {
            var remainingPoints = _points.Skip(SpawnedUnits.Count);
            Spawn(remainingPoints);
        }
        else if (_points.Count < SpawnedUnits.Count) {
            Kill(SpawnedUnits.Count - _points.Count);
        }

        for (var i = 0; i < SpawnedUnits.Count; i++)
        {
            if (i < FormationController.Instance.unitWidth)
            {
                SpawnedUnits[i].transform.position = new Vector3(
                    Mathf.Lerp(SpawnedUnits[i].transform.position.x, transform.position.x + i * 0.5f,
                        Time.deltaTime * snakeMoveSpeed),
                    SpawnedUnits[i].transform.position.y, SpawnedUnits[i].transform.position.z);
            }
            else
            {
                SpawnedUnits[i].transform.position = new Vector3(
                    Mathf.Lerp(SpawnedUnits[i].transform.position.x, SpawnedUnits[i - 3].transform.position.x,
                        Time.deltaTime * snakeMoveSpeed),
                    SpawnedUnits[i].transform.position.y, SpawnedUnits[i].transform.position.z);
            }
        }
    }

    private void Spawn(IEnumerable<Vector3> points) {
        foreach (var pos in points) {
            var unit = Instantiate(unitPrefab, transform.position + pos, Quaternion.identity, _units);
            SpawnedUnits.Add(unit);
        }
    }

    private void Kill(int num) {
        for (var i = 0; i < num; i++) {
            var unit = SpawnedUnits.Last();
            SpawnedUnits.Remove(unit);
            Destroy(unit.gameObject);
        }
    }
}