using UnityEngine;

namespace GS.Resource
{
    /// <summary>
    /// Resource table.
    /// </summary>
    [System.Serializable]
    public abstract class ResourceTable : ScriptableObject
    {
        public new string name = "New ResourceTable";
        public string[] updateEvents = { };

        /*/ Example array to be used in tables. Tag is always string.
        // Data type you can change to fit your need.
        // However, you should make new ResourceFiller script for it aswell.
        // Keep struct under class and you wont need to rename it.
        [System.Serializable]
        public struct TaggedObject { public string tag; public string data; }
        public TaggedObject[] objects;
        // */

        /// <summary>
        /// Gets the data with tag.
        /// </summary>
        /// <param name="_tag">Tag.</param>
        /// <param name="_data">Data.</param>
        public abstract void GetDataWithTag(string _tag, out object _data);

        #region Unity editor calls

#if UNITY_EDITOR
        /*// <summary>
        /// The serialized data.
        /// Used to add more data in ResourceTableEditor.
        /// Serialized data is int, float, string etc generic types.
        /// Doesn't work with structs.
        /// </summary>
        [SerializeField, HideInInspector]
        private string serializedData;
        // */

        /// <summary>
        /// Gets the type of the data object.
        /// Used to create universal data input in GS.Resource.ResourceTableEditor!
        /// </summary>
        /// <returns>The data type.</returns>
        public abstract System.Type GetDataType();

        /// <summary>
        /// Gets the first available data.
        /// UnityEditor only method.
        /// Surround with #if UNITY_EDITOR and #endif
        /// </summary>
        /// <param name="_data">Data.</param>
        public abstract void GetData(out string[] _tags, out string[] _labels);

        /// <summary>
        /// Class to the json data.
        /// </summary>
        /// <returns>The json.</returns>
        public string ToJSON()
        {
            return JsonUtility.ToJson(this, true);
        }

        /// <summary>
        /// Class from the json data.
        /// </summary>
        /// <param name="json">Json.</param>
        public void FromJSON(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
#endif // UNITY_EDITOR

        #endregion // Unity editor calls
    }
}
