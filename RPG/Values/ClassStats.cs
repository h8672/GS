using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.RPG.Values
{
    [CreateAssetMenu(fileName = "New ClassStats", menuName = "GS/Stats/ClassStats")]
    public class ClassStats : ScriptableObject
    {
        public new string name = "";
        [TextArea] public string description = "";
        
        [SerializeField] private GS.Stats.StatValuePair[] values = { };
        
        // Update class values (for example, on level up).
        public void UpdateValues(float delta)
        {
            for(int i = 0; i < values.Length; i++) {
                values[i].value = values[i].stat.GetEfficiencyValue(delta);
            }
        }
        
        // Get class values (for example to calculate total stats).
        public GS.Stats.StatValuePair[] GetValues() {
            return values;
        }

        #region Unity editor methods

#if UNITY_EDITOR
        public string ToJSON() {
            return JsonUtility.ToJson(this, true);
        }
        public void FromJSON(string json) {
            JsonUtility.FromJsonOverwrite(json, this);
        }

        public static string[] FindClassNames()
        {
            List<ClassStats> stats = new List<ClassStats>();
            stats.AddRange(Resources.LoadAll<ClassStats>(""));

            List<string> names = new List<string>();
            foreach(ClassStats stat in stats)
            {
                names.Add(stat.name);
            }
            names.TrimExcess();

            return names.ToArray();
        }

        public static string[] FindClassStatNames()
        {
            // Find all class stats
            List<ClassStats> stats = new List<ClassStats>();
            stats.AddRange(Resources.LoadAll<ClassStats>(""));

            // Get class stats
            List<string> statNames = new List<string>();
            foreach (ClassStats stat in stats)
            {
                foreach(GS.Stats.StatValuePair pair in stat.values)
                {
                    statNames.Add(pair.stat.name);
                }
            }
            statNames.TrimExcess();

            return statNames.ToArray();
        }
#endif // UNITY_EDITOR

        #endregion // Unity editor methods
    }
}
