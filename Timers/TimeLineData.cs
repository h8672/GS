using UnityEngine;

namespace GS.Timers
{
    public struct TimeLineData
    {
        public Vector3 position;
        public Quaternion rotation;

        public TimeLineData(Transform target)
        {
            position = target.position;
            rotation = target.rotation;
        }
    }
}