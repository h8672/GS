using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Stats
{
    [CreateAssetMenu(fileName = "New StatLink", menuName = "GS/Stats/StatLink")]
    public class StatLink : ScriptableObject
    {
        public GS.Stats.StatValuePair baseStat;
        [TextArea] public string description = "";
        
        [SerializeField] private GS.Stats.StatValuePair[] statConverts = { };
        
        public GS.Stats.StatValuePair[] ConvertStats(int convertValue)
        {
            List<GS.Stats.StatValuePair> convertedValues 
                = new List<GS.Stats.StatValuePair>();
            
            foreach(GS.Stats.StatValuePair oldPair in statConverts) {
                GS.Stats.StatValuePair newPair;
                newPair.stat = oldPair.stat;
                newPair.value = baseStat.value / oldPair.value * convertValue;
                convertedValues.Add(newPair);
            }
            return convertedValues.ToArray();
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
