using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GS.Resource.Table
{
    /// <summary>
    /// String table.
    /// </summary>
    [CreateAssetMenu(fileName = "New TextureTable", menuName = "GS/Resource/TextureTable")]
    public class TextureTable : GS.Resource.ResourceTable
    {
        // Example array to be used in tables. Tag is always string.
        // Data type you can change to fit your need.
        // However, you should make new LanguageFiller script for it aswell.
        // Keep struct under class and you wont need to rename it.
        [System.Serializable]
        public struct TaggedObject { public string tag;  public Texture data; }
        public TaggedObject[] objects;
        // */

        /// <summary>
        /// Gets the data with tag.
        /// </summary>
        /// <param name="_tag">Tag.</param>
        /// <param name="_data">Data.</param>
        public override void GetDataWithTag(string _tag, out object _data)
        {
            object data = null;
            for(int i = 0; i < objects.Length; i++)
            {
                if (objects[i].tag.Equals(_tag))
                {
                    data = objects[i].data;
                    break;
                }
            }
            _data = data;
        }

        #region Unity editor calls

#if UNITY_EDITOR
        /*// <summary>
        /// The serialized data.
        /// Used to add more data in ResourceTableEditor.
        /// Serialized data is int, float, string etc generic types.
        /// Doesn't work with structs.
        /// </summary>
        [SerializeField, HideInInspector]
        private Texture serializedData;
        // */

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <returns>The data type.</returns>
        public override Type GetDataType()
        {
            return typeof(Texture);
        }

        /// <summary>
        /// Gets the data tags and data labels.
        /// UnityEditor only method.
        /// Surround with #if UNITY_EDITOR and #endif
        /// </summary>
        /// <param name="_tags">Tags.</param>
        /// <param name="_labels">Data labels.</param>
        public override void GetData(out string[] _tags, out string[] _labels)
        {
            List<string> tags = new List<string>();
            List<string> labels = new List<string>();
            foreach(TaggedObject obj in objects)
            {
                tags.Add(obj.tag);
                labels.Add(obj.data.name);
            }
            _tags = tags.ToArray();
            _labels = labels.ToArray();
            tags.Clear();
            labels.Clear();
        }
#endif // UNITY_EDITOR

        #endregion // Unity editor calls
    }
}