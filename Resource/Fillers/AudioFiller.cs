using System;
using UnityEngine;

namespace GS.Resource.Filler
{
    /// <summary>
    /// Text filler.
    /// Updates the text of TextMeshProUGUI component.
    /// </summary>
    [AddComponentMenu("GS/Resource/AudioFiller")]
    [RequireComponent(typeof(AudioSource))]
    public class AudioFiller : ResourceFiller
    {
#if UNITY_EDITOR
        public override Type[] GetRequiredTableTypeOptions()
        {
            return new System.Type[] { typeof(GS.Resource.Table.AudioTable) };
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
            AudioClip data = null;
            if ( table == null) {
                // Failed to find language table
                Debug.LogError("AudioFiller didn't find the audio table.", this);
            } else {
                if ( table.GetDataType().Equals(typeof(AudioClip)) ) {
                    table.GetDataWithTag(this.selectedTag, out object _data);
                    data = (AudioClip)_data;
                } else {
                    Debug.LogError("Received wrong type of ResourceTable!", this);
                }
            }
            gameObject.GetComponent<UnityEngine.AudioSource>().clip = data;//.text = text;
        }
    }
}
