using UnityEngine;

namespace GS.Resource
{
    /// <summary>
    /// Resource folder.
    /// Contains name, path, resource and ResourceTables.
    /// </summary>
    [CreateAssetMenu(fileName = "New ResourceFolder", menuName = "GS/Resource/ResourceFolder")]
    public class ResourceFolder : ScriptableObject
    {
        [Header("Folder data")]
        [Tooltip("Path in Assets/Resources/~")]
        public string path = "ResourceLocation";

        [Tooltip("Main event name for this resource. Invokes all objects to update that still has it.")]
        public string resourceKey = "ResourceKey";

        [Tooltip("Resource tree branch. Also the folder, under the path, where new ResourceTables are collected.")]
        public string resource = "Default";

        public GS.Resource.ResourceTable[] tables;

        /// <summary>
        /// Gets the table with the name.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_table">Table.</param>
        public void GetTableWithName(string _name, out GS.Resource.ResourceTable _table)
        {
            GS.Resource.ResourceTable table = null;
            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].name.Equals(_name))
                {
                    table = tables[i];
                }
            }
            _table = table;
        }

        #region Unity editor calls

#if UNITY_EDITOR
        /// <summary>
        /// Tries the get table from the folder.
        /// If table doesn't exist, return null.
        /// </summary>
        /// <param name="_table">Table.</param>
        public void TryGetTable(out GS.Resource.ResourceTable _table)
        {
            if (tables.Length > 0) {
                _table = tables[0];
                return;
            }
            _table = null;
        }

        public string ToJSON()
        {
            return JsonUtility.ToJson(this, true);
        }
        public void FromJSON(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
#endif // UNITY_EDITOR

    #endregion // Unity editor calls
    }
}