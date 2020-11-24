using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Data
{
    [System.Serializable]
    public class ObjectData : ScriptableObject
    {
        public new string name;

        [TextArea]
        public string description;

        [Header("Icon")]
        public Texture iconTexture;
        public Texture borderTexture;
    }
}