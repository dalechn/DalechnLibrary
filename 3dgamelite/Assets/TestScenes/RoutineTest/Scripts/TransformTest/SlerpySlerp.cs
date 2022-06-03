using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts {
    public class SlerpySlerp : MonoBehaviour {
        [SerializeField] private Transform _start, _center, _end;
        [SerializeField] private int _count = 15;
        private void OnDrawGizmos() {
            foreach (var point in EvaluateSlerpPoints(_start.position, _end.position, _center.position,_count)) {
                Gizmos.DrawSphere(point, 0.1f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_center.position, 0.2f);
        }

        IEnumerable<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end, Vector3 center,int count = 10) {
            var startRelativeCenter = start - center;
            var endRelativeCenter = end - center;

            var f = 1f / count;

            for (var i = 0f; i < 1 + f; i += f) {
                yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + center;
            }
        }
    }


    //-------------------------------------Slerp未知:暂时还不理解
    //Vector3 center = (startMarker.position + endMarker.position) * 0.5F;
    //center -= new Vector3(0, 1, 0);
    //Vector3 riseRelCenter = startMarker.position - center;
    //Vector3 setRelCenter = endMarker.position - center;

    //tempPos = Vector3.Slerp(riseRelCenter, setRelCenter, fractionOfJourney);
    //tempPos += center;

}