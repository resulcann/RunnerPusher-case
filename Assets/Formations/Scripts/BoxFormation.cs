using System.Collections.Generic;
using UnityEngine;

namespace Formations.Scripts
{
    public class BoxFormation : FormationBase
    {
        [SerializeField] private int numberOfUnit = 1;
        [SerializeField] private int unitWidth = 5;
        [SerializeField] private int unitDepth = 5;

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
}