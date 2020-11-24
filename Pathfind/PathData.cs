using UnityEngine;

namespace GS.Data
{
    public struct PathData
    {
        public Vector3 startPosition;
        public Vector3 endPosition;
        public AnimationCurve curve;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GS.Data.PathData"/> struct.
        /// </summary>
        public PathData(Vector3 _startPosition, Vector3 _endPosition, AnimationCurve _curve)
        {
            startPosition = _startPosition;
            endPosition = _endPosition;
            curve = _curve;
        }
    }
}