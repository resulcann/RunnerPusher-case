using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Formations.Scripts
{
    public class ExampleArmy : MonoBehaviour {
        private FormationBase _formation;

        public FormationBase Formation {
            get {
                if (_formation == null) _formation = GetComponent<FormationBase>();
                return _formation;
            }
            set => _formation = value;
        }

        [SerializeField] private GameObject unitPrefab;
        [SerializeField] private float unitSpeed = 2;
        [SerializeField] private float snakeMoveSpeed = 20;
    

        private readonly List<GameObject> _spawnedUnits = new List<GameObject>();
        private List<Vector3> _points = new List<Vector3>();
        private Transform _units;

        private void Awake() {
            _units = new GameObject("Units").transform;
            _units.parent = transform;
        }

        private void Update() 
        {
            SetFormation();
        }

        private void SetFormation() {
            _points = Formation.EvaluatePoints().ToList();

            if (_points.Count > _spawnedUnits.Count) {
                var remainingPoints = _points.Skip(_spawnedUnits.Count);
                Spawn(remainingPoints);
            }
            else if (_points.Count < _spawnedUnits.Count) {
                Kill(_spawnedUnits.Count - _points.Count);
            }
        
            if (_spawnedUnits.Count > 3)
            {
                for (var i = 3; i < _spawnedUnits.Count; i++) {
                    //_spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position, transform.position + _points[i], _unitSpeed * Time.deltaTime);
                    _spawnedUnits[i].transform.position = new Vector3(Mathf.Lerp(_spawnedUnits[i].transform.position.x, _spawnedUnits[i-3].transform.position.x,Time.deltaTime * snakeMoveSpeed),
                        _spawnedUnits[i].transform.position.y, _spawnedUnits[i].transform.position.z);
                }
            }
        }

        private void Spawn(IEnumerable<Vector3> points) {
            foreach (var pos in points) {
                var unit = Instantiate(unitPrefab, transform.position + pos, Quaternion.identity, _units);
                _spawnedUnits.Add(unit);
            }
        }

        private void Kill(int num) {
            for (var i = 0; i < num; i++) {
                var unit = _spawnedUnits.Last();
                _spawnedUnits.Remove(unit);
                Destroy(unit.gameObject);
            }
        }
    }
}