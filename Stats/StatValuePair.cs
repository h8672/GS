using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Stats
{
    [System.Serializable]
    public struct StatValuePair {
        public GS.Stats.StatDefinition stat;
        public float value;
    }
}
