using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Stats
{
    [CreateAssetMenu(fileName = "New StatDefinition", menuName = "GS/Stats/StatDefinition")]
    public class StatDefinition : GS.Data.ObjectData
    {
        [SerializeField] private float minValueCap = 0f, maxValueCap = 1f;
        [SerializeField] private AnimationCurve efficiencyCurve = new AnimationCurve(new[] {
            new Keyframe(0f, 0f, 0f, 0f, 0f, 0f), new Keyframe(1f, 1f, 0f, 0f, 0f, 0f)
        });
        
        public float GetEfficiencyValue(float curvePoint)
        {
            // Returns the height of curve as efficiency.
            return Mathf.Clamp(
                efficiencyCurve.Evaluate(curvePoint),
                minValueCap, maxValueCap
            );
        }

        #region Unity editor methods

#if UNITY_EDITOR
        public string ToJSON()
        {
            return JsonUtility.ToJson(this, true);
        }
        public void FromJSON(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
#endif // UNITY_EDITOR

        #endregion // Unity editor methods
    }
}
