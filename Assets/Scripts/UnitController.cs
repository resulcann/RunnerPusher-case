using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private TextMeshProUGUI currentCrowdNumber;
    public Image crowdNumberImg;
    private int _tempRow = 1;


    public readonly List<Unit> SpawnedUnits = new List<Unit>();
    private List<Vector3> _points = new List<Vector3>();
    private Transform _units;
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Sitting = Animator.StringToHash("Sitting");

    private void Awake()
    {
        Instance = this;
        _units = new GameObject("Units").transform;
        _units.parent = transform.parent;
    }

    private void Start()
    {
        currentCrowdNumber.text = SpawnedUnits.Count.ToString();
    }
    
    // update methodu içerisinde kullanarak lerp içinde Time.delta time kullanmazsan donma olmuyor.

    private void FixedUpdate()
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
                        snakeMoveSpeed * Time.deltaTime),
                    SpawnedUnits[i].transform.position.y, SpawnedUnits[i].transform.position.z);
            }
            else
            {
                SpawnedUnits[i].transform.position = new Vector3(
                    Mathf.Lerp(SpawnedUnits[i].transform.position.x, SpawnedUnits[i - 3].transform.position.x,
                        snakeMoveSpeed * Time.deltaTime),
                    SpawnedUnits[i].transform.position.y, SpawnedUnits[i].transform.position.z);
            }
        }
    }
    private void Spawn(IEnumerable<Vector3> points) {
        foreach (var pos in points) {
            var unit = Instantiate(unitPrefab, transform.position + pos, Quaternion.identity, _units);
            SpawnedUnits.Add(unit);
            unit.stickManColor = SpawnedUnits.First().stickManColor;
            unit.renderer.material = SpawnedUnits.First().renderer.material;
            currentCrowdNumber.text = SpawnedUnits.Count.ToString();
            if(GameManager.Instance.gameState == GameState.Gameplay) unit.GetComponent<Animator>().SetBool(Run,true);
        }
    }
    private void Kill(int num) {
        for (var i = 0; i < num; i++) {
            var unit = SpawnedUnits.Last();
            SpawnedUnits.Remove(unit);
            Destroy(unit.gameObject);
            currentCrowdNumber.text = SpawnedUnits.Count.ToString();
        }
    }

    public IEnumerator TowerFormation()
    {
        var posY = 1;
        var cameraFollower = GameManager.Instance.mainCam.GetComponent<CameraFollower>();
        snakeMoveSpeed = 100f;
        cameraFollower.SetFinishCameraPos(1f);
        crowdNumberImg.gameObject.SetActive(false);

        if (SpawnedUnits.Count > 3)
        {
            for (var i = 3; i < SpawnedUnits.Count; i++)
            {
                var currentPos = SpawnedUnits[i].transform.localPosition;
            
                SpawnedUnits[i].transform.DOLocalMove(new Vector3(SpawnedUnits[i-3].transform.localPosition.x, currentPos.y + posY, 
                    SpawnedUnits[0].transform.localPosition.z), 0.1f);
                SpawnedUnits[i].GetComponent<Animator>().SetBool(Sitting,true);
                yield return new WaitForSeconds(0.05f);
            
                if (_tempRow % 3 == 0)
                {
                    posY++;
                    cameraFollower.SetFinishCameraPos(1f);
                    _tempRow = 1;
                }
                else _tempRow++;
            }
        }
    }
}