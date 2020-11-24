using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.RPG.Values
{

    /*/ Commented
    public enum ItemQuality
    {
        Trash,
        Unfinished,
        Counterfeit,
        Failure,
        Normal,
        Superior,
        Expectional,
    }
    public enum ObjectRarity
    {
        Flawed,
        Normal,
        Uncommon,
        Rare,
        Legendary,
        Unique,
    } // */


    [CreateAssetMenu(fileName = "New ItemStats", menuName = "GS/Stats/ItemStats")]
    public class ItemStats : GS.Data.ObjectData
    {
        [SerializeField] private GS.Stats.StatValuePair[] values = { };
        
        // Get class values (for example to calculate total stats).
        public GS.Stats.StatValuePair[] GetValues() {
            return values;
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

        public static string[] FindItemNames()
        {
            List<ItemStats> stats = new List<ItemStats>();
            stats.AddRange(Resources.LoadAll<ItemStats>(""));

            List<string> names = new List<string>();
            foreach (ItemStats stat in stats)
            {
                names.Add(stat.name);
            }
            names.TrimExcess();

            return names.ToArray();
        }

        public static string[] FindItemStatNames()
        {
            // Find all class stats
            List<ItemStats> stats = new List<ItemStats>();
            stats.AddRange(Resources.LoadAll<ItemStats>(""));

            // Get class stats
            List<string> statNames = new List<string>();
            foreach (ItemStats stat in stats)
            {
                foreach (GS.Stats.StatValuePair pair in stat.values)
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
