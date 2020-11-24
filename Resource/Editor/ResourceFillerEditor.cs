using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GS.Editor.Resource
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GS.Resource.ResourceFiller), true)]
    public class ResourceFillerEditor : UnityEditor.Editor
    {
        private string[] _tableOptions = { "EmptyAssets" };
        private string[] _itemOptions = { "Empty" };
        private string _previewText = "Preview text";

        private int _tableOption = 0;
        private int _tagOption = 0;

        private GS.Resource.ResourceTable[] _tables;

        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            if (_tables == null)
            {
                EditorGUILayout.LabelField("ResourceFiller updates components on start.");
                // Get the missing tables and start to edit values.
                if (GUILayout.Button("Edit values!"))
                {
                    GetTables();
                }

                // Save current changes in editor GUI
                EditorUtility.SetDirty(this);
            }
            else if (_tables.Length.Equals(0))
            {
                EditorGUILayout.LabelField("No tables found for current language.");
                if (GUILayout.Button("Try again!"))
                {
                    GetTables();
                }

                // Save current changes in editor GUI
                EditorUtility.SetDirty(this);
            }
            else
            {
                EditorGUILayout.Space();

                // Check resource table asset
                List<string> temporaryStrings = new List<string>();
                foreach (GS.Resource.ResourceTable obj in _tables)
                {
                    temporaryStrings.Add(obj.name);
                }
                _tableOptions = temporaryStrings.ToArray();
                temporaryStrings.Clear();

                _tableOption = EditorGUILayout.Popup("Select Table", _tableOption, _tableOptions);

                // Update table related values! Resets tag option on new selection.
                if ( _tables[_tableOption].name != serializedObject.FindProperty("selectedTable").stringValue )
                {
                    serializedObject.FindProperty("selectedTable").stringValue = _tables[_tableOption].name;
                    _tagOption = 0;
                }

                Debug.LogWarning("Going through table data");
                // Check resource table
                if (_tables[_tableOption] != null)
                {
                    _tables[_tableOption].GetData(
                        out string[] _tags,
                        out string[] _labels
                    );

                    if (_tags.Length > 0)
                    {
                        _itemOptions = _tags;
                    }

                    _tagOption = EditorGUILayout.Popup("Select table tag", _tagOption, _itemOptions);

                    serializedObject.FindProperty("selectedTag").stringValue = _itemOptions[_tagOption];

                    if (_labels.Length > 0)
                    {
                        _previewText = _labels[_tagOption];
                    }
                }

                EditorGUILayout.LabelField("Content preview", _previewText);

                // Save current changes on property values.
                serializedObject.ApplyModifiedProperties();

                // Save current changes in editor GUI
                EditorUtility.SetDirty(this);
            }
        }

        void OnGUI()
        {
            EditorUtility.SetDirty(target);
        }

        private void GetTables()
        {
            var _class = target as GS.Resource.ResourceFiller;
            _class.GetFolderForEditor(out GS.Resource.ResourceFolder _folder);

            if (_folder == null)
            {
                Debug.Log("GS.Resource.ResourceFolder asset has not been found. Create one.\nRecommended path '/Assets/Resources/Language/'.\nFor further trouble check the README.md file or send me a message.", this);
                return;
            }

            if (_folder.tables.Length.Equals(0))
            {
                Debug.Log("GS.Resource.ResourceFolder asset missing from TextFolder.\nCreate one and then select it in ResourceFiller component.", this);
                //return;
            }

            List<GS.Resource.ResourceTable> tables = new List<GS.Resource.ResourceTable>();
            foreach (GS.Resource.ResourceTable table in _folder.tables)
            {
                bool content = false;
                foreach (System.Type type in _class.GetRequiredTableTypeOptions())
                {
                    if ( table.GetType().Equals(type) )
                    {
                        content = true;
                    }
                }
                if (content) { tables.Add(table); }
            }
            _tables = tables.ToArray();

            EditorUtility.SetDirty(target);
        }
    }
}