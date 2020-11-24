using System.Collections.Generic;
using UnityEngine;

namespace GS.Data
{
    public class ResourceEventSwitch : MonoBehaviour
    {
        public string keyValue = "ResourcePath";
        public string defaultResource = "Default";

        public TMPro.TMP_InputField keyValueText;
        public TMPro.TextMeshProUGUI resourceText;
        public TMPro.TextMeshProUGUI eventText;

        [SerializeField] private Color32 currentColor = new Color32(0, 255, 0, 255);
        [SerializeField] private Color32 targetColor = new Color32();

        [SerializeField] private string[] resources = { };
        [SerializeField] private string[] events = { };
        private int selectionResource = 0, selectionEvent = 0;

        void Start()
        {
            if (!GS.Data.Settings.Instance.keyValues.HasKey(keyValue)) {
                GS.Data.Settings.Instance.keyValues.SetKey(keyValue, defaultResource);
            }
            GetData();
            UpdateText();
        }

        #region Private methods

        private void GetData()
        {
            GS.Resource.ResourceFolder[] _folders = Resources.LoadAll<GS.Resource.ResourceFolder>("");

            List<string> _resources = new List<string>();
            List<string> _events = new List<string>();
            if (_folders.Length.Equals(0))
            {
                _resources.Add(defaultResource);
            }
            else
            {
                foreach (GS.Resource.ResourceFolder folder in _folders)
                {
                    if (!_resources.Contains(folder.resource))
                    {
                        _resources.Add(folder.resource);
                        foreach(GS.Resource.ResourceTable _table in folder.tables)
                        {
                            foreach(string _tableEvent in _table.updateEvents)
                            {
                                if (!_events.Contains(_tableEvent))
                                {
                                    _events.Add(_tableEvent);
                                }
                            }
                        }
                    }

                }
            }
            resources = _resources.ToArray();
            _resources.Clear();
            events = _events.ToArray();
            _events.Clear();
        }

        private void UpdateText()
        {
            GS.Data.Settings.Instance.keyValues.TryGetKeyValue(keyValue, out string value);
            if (keyValueText != null)
            {
                keyValueText.text = keyValue;
            }

            if (resourceText != null)
            {
                resourceText.color = (
                    resources[selectionResource].Equals(value)
                    ? currentColor : targetColor
                );

                resourceText.text = resources[selectionResource];
            }

            if (eventText != null)
            {
                eventText.text = events[selectionEvent];
            }
        }

        #endregion // Private methods
        #region Button methods

        public void ResourceLeft()
        {
            selectionResource -= 1;
            if (selectionResource < 0) { selectionResource = resources.Length - 1; }
            UpdateText();
        }
        public void ResourceRight()
        {
            selectionResource += 1;
            if (selectionResource >= resources.Length) { selectionResource = 0; }
            UpdateText();
        }

        public void EventLeft()
        {
            selectionEvent -= 1;
            if (selectionEvent < 0) { selectionEvent = events.Length - 1; }
            UpdateText();
        }
        public void EventRight()
        {
            selectionEvent += 1;
            if (selectionEvent >= events.Length) { selectionEvent = 0; }
            UpdateText();
        }

        public void UpdateEvent()
        {
            GS.Data.Settings.Instance.keyValues.SetKey(keyValue, defaultResource);
            GS.Data.EventManager.TriggerEvent(events[selectionEvent]);
        }

        public void SetDefaultLanguage()
        {
            UpdateEvent();
            UpdateText();
        }
        public void SetLanguage()
        {
            UpdateEvent();
            UpdateText();
        }

        #endregion // Button methods
    }
}