using UnityEngine;

namespace GS.Data
{
    public class ObjectStatus : MonoBehaviour
    {
        [SerializeField]
        private GS.Data.ObjectData _object = null;

        public GS.Data.ObjectData Object
        {
            get { return _object; }
        }
    }
}
