using System.Collections.Generic;
using UnityEngine;

public class FormationController : FormationBase
{
    public static FormationController Instance { get; private set; }
    public int numberOfUnit = 1;
    public int unitWidth = 3;
    public int unitDepth = 5;

    private void Awake()
    {
        Instance = this;
    }

    public override IEnumerable<Vector3> EvaluatePoints() {
        var middleOffset = new Vector3(unitWidth * 0.5f, 0, unitDepth * 0.5f);
            
        for (var i = 0; i < numberOfUnit; i++) {
            var pos = new Vector3(i % unitWidth, 0, -Mathf.Ceil(i/unitWidth));

            pos -= new Vector3(middleOffset.x, 0, 0);

            pos += GetNoise(pos);

            pos *= Spread;

            yield return pos;
        }
    }
}