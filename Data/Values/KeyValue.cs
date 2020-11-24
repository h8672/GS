namespace GS.Data.Values
{
    [System.Serializable]
    public struct KeyValue
    {
        public string key;
        public string value;

        public KeyValue(string _key, string _value)
        {
            key = _key;
            value = _value;
        }
    }
}