using System.Collections.Generic;
using UnityEngine;

namespace GS.Data
{
    public static class AnimationCurveParser
    {
        /// <summary>Gets the curve deltas.</summary>
        /// <returns>The curve deltas.</returns>
        /// <param name="_curve">Animation curve.</param>
        /// <param name="_accuracy">Accuracy in decimals, lower value improves accuracy.</param>
        public static float[] GetCurveDeltas(AnimationCurve _curve, float _accuracy)
        {
            float earlierValue = 0f, accuracy = 1f / _accuracy;
            List<float> deltas = new List<float>();
            for (int i = 0; i < accuracy; i++)
            {
                // Get current curve value in point i
                float value = _curve.Evaluate(_accuracy * i);
                // Get delta between points, always positive.
                deltas.Add((earlierValue >= value) ? (earlierValue - value) : (value - earlierValue));
                earlierValue = value;
            }
            return deltas.ToArray();
        }

        /// <summary>
        /// Gets the curve directions from path.
        /// </summary>
        /// <returns>The curve directions.</returns>
        /// <param name="_curve">Animation curve.</param>
        /// <param name="_accuracy">Accuracy in decimals, lower value improves accuracy.</param>
        public static Vector2[] GetCurveDirections(AnimationCurve _curve, float _accuracy)
        {
            List<Vector2> vectors = new List<Vector2>();
            foreach (float delta in GetCurveDeltas(_curve, _accuracy))
            {
                vectors.Add(new Vector2(_accuracy, delta));
            }
            return vectors.ToArray();
        }

        /// <summary>Gets the length of curve with a decimal.</summary>
        /// <returns>The accurate length of the curve.</returns>
        /// <param name="_curve">Animation curve.</param>
        /// <param name="_accuracy">Accuracy in decimals, lower value improves accuracy.</param>
        public static float GetAccurateCurveLength(AnimationCurve _curve, float _accuracy)
        {
            float
                returnValue = 0f,
                earlierValue = 0f,
                accuracy = 1f / _accuracy;

            // Integral values from path.
            for (int i = 0; i < accuracy; i++)
            {
                // Get current curve value in point i
                float value = _curve.Evaluate(_accuracy * i);
                // Get delta between points, always positive.
                returnValue += ((earlierValue >= value) ? (earlierValue - value) : (value - earlierValue));
                earlierValue = value;
            }
            // Possibly fixes the accuracy? If path changes a lot, doesn't really do anything.
            return returnValue * (1 + _accuracy);
        }
    }
}