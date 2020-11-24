using UnityEngine;

namespace GS.Resource.Filler
{
    /// <summary>
    /// Text filler.
    /// Updates the text of TextMeshProUGUI component.
    /// </summary>
    [AddComponentMenu("GS/Resource/ImageFiller")]
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class ImageFiller : ResourceFiller
    {
#if UNITY_EDITOR
        public override System.Type[] GetRequiredTableTypeOptions()
        {
            return new System.Type[] {
                typeof(GS.Resource.Table.SpriteTable),
                typeof(GS.Resource.Table.TextureTable)
            };
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
            Sprite data = null;
            if ( table == null) {
                // Failed to find language table
                Debug.LogError("ImageFiller didn't find the Texture or Sprite table.", this);
            } else {
                table.GetDataWithTag(this.selectedTag, out object _data);
                if ( table.GetDataType().Equals(typeof(Sprite)) ) {
                    data = (Sprite) _data;
                }
                else if ( table.GetDataType().Equals(typeof(Texture)) ) {
                    Texture2D tex = (Texture2D) _data;
                    data = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
                } else {
                    Debug.LogError("Received wrong type of ResourceTable!", this);
                }
            }
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = data;
        }
    }
}
