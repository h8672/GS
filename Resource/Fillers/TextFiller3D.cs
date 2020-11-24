using UnityEngine;

namespace GS.Resource.Filler
{
    /// <summary>
    /// Text filler.
    /// Updates the text of TextMeshProUGUI component.
    /// </summary>
    [AddComponentMenu("GS/Resource/TextFiller3D")]
    [RequireComponent(typeof(TMPro.TextMeshPro))]
    public class TextFiller3D : ResourceFiller
    {
#if UNITY_EDITOR
        public override System.Type[] GetRequiredTableTypeOptions()
        {
            return new System.Type[] { typeof(GS.Resource.Table.StringTable) };
        }
#endif // UNITY_EDITOR

        // Friendly reminder about shared values.
        // [SerializeField] protected string selectedTag;
        // protected GS.Resource.ResourceTable table;

        /// <summary>
        /// Updates the resource.
        /// </summary>
        protected override void UpdateFill()
        {
            string data;
            if ( table == null) {
                data = "No table data available!";
                Debug.LogError("TextFiller didn't find the string table.", this);
            }
            else {
                if ( table.GetDataType().Equals(typeof(string)) ) {
                    table.GetDataWithTag(this.selectedTag, out object _data);
                    data = (string)_data;
                } else {
                    data = "Table data type is wrong.";
                    Debug.LogError("Received wrong type of ResourceTable!", this);
                }
            }
            gameObject.GetComponent<TMPro.TextMeshPro>().text = data;
        }
    }
}
