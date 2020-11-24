using System.Collections.Generic;

namespace GS.Data.Values
{
    /// <summary>
    /// Serialized key values.
    /// </summary>
    [System.Serializable]
    public class SerializedKeyValues
    {
        [UnityEngine.SerializeField]
        private GS.Data.Values.KeyValue[] objects = { };

        private int FindKeyPosition(string _key)
        {
            int x = -1;
            while (x < objects.Length - 1)
            {
                if (objects[++x].key.Equals(_key)) { break; }
            }
            return x;
        }

        public bool HasKey(string _key)
        {
            return (FindKeyPosition(_key) > 0);
        }

        public void TryGetKeyValue(string _key, out string _value)
        {
            int x = FindKeyPosition(_key);
            _value = (x > -1 ? objects[x].value : "");
        }

        public void SetKey(string _key, string _value)
        {
            int x = FindKeyPosition(_key);

            if (x > -1)
            {
                objects[x].value = _value;
            }
            else
            {
                List<GS.Data.Values.KeyValue> _objects = new List<GS.Data.Values.KeyValue>();

                _objects.AddRange(objects);
                _objects.Add(new GS.Data.Values.KeyValue(_key, _value));

                objects = _objects.ToArray();
            }
        }
    }
}