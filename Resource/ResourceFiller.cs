using UnityEngine;

namespace GS.Resource
{
    // Example code for different usages.
    // [AddComponentMenu("GS/Language/TextFiller")]
    // [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    // [RequireComponent(typeof(AudioSource))]
    // gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Localize();

    /// <summary>
    /// Resource filler.
    /// Requires implementation for different ResourceTable types.
    /// Implements all required basic methods when language changes.,
    /// 
    /// Use preference key to give resource additional layer.
    /// The value set there is fetch value from player preferences.
    /// For example:
    /// - Language (global localization)
    /// - LanguageUI (localized UI)
    /// - LanguageVoice (localized voice clips)
    /// - LanguageGame (localized in game)
    /// - LanguageNPC (localized npcs)
    /// - LanguageRegion (localized region)
    /// </summary>
    public abstract class ResourceFiller : MonoBehaviour
    {
        [SerializeField] private string keyValue = "Resource";
        [SerializeField] private string selectedTable = "";
        [SerializeField] protected string selectedTag = "";
        protected GS.Resource.ResourceTable table;

        /// <summary>
        /// Updates the resource.
        /// Always called when resource event updates.
        /// </summary>
        protected abstract void UpdateFill();

        #region Private methods

        /// <summary>
        /// Gets the resource folder.
        /// </summary>
        /// <param name="_folder">Folder.</param>
        private void GetResourceFolder(out GS.Resource.ResourceFolder _folder)
        {
            GS.Resource.ResourceFolder[] _folders = Resources.LoadAll<GS.Resource.ResourceFolder>("");
            Debug.Log("Folder count: " + _folders.Length);
            if ( _folders.Length.Equals(0) )
            {
                Debug.LogError("No TextFolder found!\nMake sure you have TextFolder under Assets/Resources/ directory.\nRecommended path '/Assets/Resources/~'.\n", this);
                _folder = null;
                return;
            }

            GS.Resource.ResourceFolder folder = null;
            for(int i = 0; i < _folders.Length; i++)
            {
                GS.Data.Settings.Instance.keyValues.TryGetKeyValue(keyValue, out string value);

                if ( _folders[i].resource.Equals(value) ) {
                    folder = _folders[i];
                    Debug.Log("Found ResourceFolder using given KeyValue: " + keyValue);
                    break;
                }
            }
            Debug.Log("Returning ResourceFolder");
            _folder = folder;
        }

        /// <summary>
        /// Updates the resource table.
        /// </summary>
        private void UpdateResourceTable()
        {
            // If player preferences has no key for resource, set one.
            if (!GS.Data.Settings.Instance.keyValues.HasKey(keyValue))
            {
                GS.Data.Settings.Instance.keyValues.SetKey(keyValue, "Default");
            }

            GetResourceFolder(out GS.Resource.ResourceFolder folder);

            if (folder != null)
            {
                folder.GetTableWithName(selectedTable, out table);
                if (table == null)
                {
                    Debug.LogError(string.Format("Missing ResourceTable '{0}'", selectedTable));
                }
            }
            else
            {
                Debug.LogError(string.Format("Missing ResourceFolder '{0}'", selectedTable));
            }
            UpdateFill();
        }

        /// <summary>
        /// Listens the events.
        /// </summary>
        private void ListenEvents()
        {
            UpdateResourceTable();
            if (table == null || table.updateEvents.Length.Equals(0)) { return; }
            foreach (string _event in table.updateEvents)
            {
                GS.Data.EventManager.StartListening(_event, UpdateResourceTable);
            }
        }

        /// <summary>
        /// Stops the listening.
        /// </summary>
        private void StopListening()
        {
            if (table == null || table.updateEvents.Length.Equals(0)) { return; }
            foreach (string _event in table.updateEvents)
            {
                GS.Data.EventManager.StopListening(_event, UpdateResourceTable);
            }
        }

        #endregion // Private methods
        #region Unity calls

        void OnEnable() { ListenEvents(); }
        void OnDisable() { StopListening(); }

        #endregion // Unity calls
        #region Unity editor calls

#if UNITY_EDITOR
        /// <summary>
        /// Gets the required table type options.
        /// </summary>
        /// <returns>The required table type options.</returns>
        public abstract System.Type[] GetRequiredTableTypeOptions();

        /// <summary>
        /// Gets the folder for Unity Editor.
        /// </summary>
        /// <param name="_folder">Folder.</param>
        public void GetFolderForEditor(out GS.Resource.ResourceFolder _folder)
        {
            // If player preferences has no key for language, set one.
            if (!GS.Data.Settings.Instance.keyValues.HasKey(keyValue))
            {
                GS.Data.Settings.Instance.keyValues.SetKey(keyValue, "Default");
            }

            GetResourceFolder(out GS.Resource.ResourceFolder folder);
            _folder = folder;
        }

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
