using System.Collections.Generic;
using UnityEngine;

namespace GS.Data
{
    /// <summary>
    /// Path data parser.
    /// </summary>
    /// Contained methods:
    /// - GetCurveDeltas
    /// - GetCurveDirections
    /// - GetAccurateCurveLength
    /// - GetAccuratePathLength
    public static class PathDataParser
    {
        /// <summary>
        /// Gets the accurate length of the path.
        /// </summary>
        /// <returns>The accurate path length.</returns>
        /// <param name="_path">Path data.</param>
        /// <param name="_accuracy">Accuracy.</param>
        public static float GetAccuratePathLength(PathData _path, float _accuracy)
        {
            return AnimationCurveParser.GetAccurateCurveLength(_path.curve, _accuracy)
                * Vector3.Distance(_path.startPosition, _path.endPosition);
        }

        /// <summary>
        /// Gets the path directions.
        /// </summary>
        /// <returns>The path directions.</returns>
        /// <param name="_path">Path data.</param>
        /// <param name="_accuracy">Accuracy in decimals, lower value improves accuracy.</param>
        public static Vector3[] GetPathDirections(PathData _path, float _accuracy)
        {
            /// <summary>
            /// Get angle difference between routes.
            /// Negative value, turns to left. Positive value, turns to right.
            /// </summary>
            /// 
            /// Research on angle between world direction and curve direction.
            /// 
            /// If both values are negative...
            ///  -7f - -5f = -2f      Expected value
            ///  -5f - -7f =  2f      Expected value
            /// 
            /// If first value is positive...
            ///   7f - -5f =  12f     Opposite direction of Expected value
            ///   5f - -7f =  12f     Opposite direction of Expected value
            /// 
            /// If second value is positive...
            ///  -7f -  5f = -12f     Opposite direction of Expected value
            ///  -5f -  7f = -12f     Opposite direction of Expected value
            /// 
            /// First value should be the value turned in this equation.
            /// Second value is to help with the turn to right direction.
            /// If both values are negative, resulting angle is in right direction.
            /// If values are in opposite directions, resulting angle is also opposite.
            /// 
            /// Direction should be changed when only other value is negative.
            /// Repeating calculation is curve angle - direction angle.
            /// If there's pair of different directions, turn the resulting angle aswell.
            /// 
            /// If statement explanation: Option 1 OR Option 2
            /// 1: Curve angle is larger or same as 0f which is larger or same as direction angle
            /// 2: Curve angle is smaller than 0f which is smaller than direction angle
            /// Alternative: NOT (Option 1 OR Option 2)
            /// 1: both curve and direction angles are smaller than 0f
            /// 2: both curve and direction angles are larger or same as 0f
            /// 
            /// Programmatically alternative statement has extra NOT.
            /// 
            /// Result, true when values are in different ends of 0f, positive and negative sides.
            /// In math 0f is considered positive value.
            /// 
            /// Great usage of 3 hours... Study © Juha-Matti Kokkonen 11.02.2019

            // Get path direction
            Vector3 pathDirection = _path.endPosition - _path.startPosition;
            pathDirection = new Vector3(pathDirection.x, 0f, pathDirection.z);

            // Get curve direction
            Vector3 curveDirection = new Vector3(
                _path.curve.keys[_path.curve.keys.Length].time, 0f,
                _path.curve.keys[_path.curve.keys.Length].value
            );

            float
                curveAngle = Vector3.Angle(Vector3.forward, curveDirection),
                directionAngle = Vector3.Angle(Vector3.forward, pathDirection),
                angle = curveAngle - directionAngle;

            if ((curveAngle >= 0f && 0f > directionAngle) || (curveAngle < 0f && 0f <= directionAngle))
            {
                angle = -angle;
            }

            // Create Quaternion which turns all vectors with the angle.
            Quaternion turner = Quaternion.AngleAxis(angle, Vector3.up);

            // Get curve directions.
            Vector2[] points = AnimationCurveParser.GetCurveDirections(_path.curve, _accuracy);

            // Get distance between points
            float distance = Vector3.Distance(_path.startPosition, _path.endPosition);

            // Change curve directions to represent 3D world directions
            List<Vector3> directions = new List<Vector3>();
            foreach (Vector2 point in points)
            {
                // Use Quaternion to rotate curve closer to 3D world direction.
                directions.Add(turner * new Vector3(point.x, 0f, point.y) * distance);
            }
            return directions.ToArray();
        }
    }
}