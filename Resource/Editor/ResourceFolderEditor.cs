using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace GS.Editor.Resource
{
    [CustomEditor(typeof(GS.Resource.ResourceFolder))]
    public class ResourceFolderEditor : UnityEditor.Editor
    {
        private string _resource = "Not Default";
        private string _jsonData = "Json data";
        private bool fold;

        public override void OnInspectorGUI ()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            GUILayout.Label("");

            if (GUILayout.Button("Quick edit tools")) { fold = !fold; }

            if (!fold)
            {
                GUILayout.Label("You can manage assets with drag and drop to Assets array if there's problems.");
            }
            else
            {
                DrawAssetSelection();

                if (GUILayout.Button("Find ResourceTable assets from directory"))
                {
                    FindAssets();
                }

                GUILayout.Label("\nCopy this resource tree as a new resource.");
                EditorGUILayout.BeginHorizontal();
                _resource = GUILayout.TextArea(_resource);
                if (GUILayout.Button("Copy resources"))
                {
                    CopyAssets();
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Label("\nJson data, easy import and export.");

                EditorGUILayout.BeginHorizontal();
                var _class = target as GS.Resource.ResourceFolder;
                if (GUILayout.Button("Export class to Json"))
                {
                    _jsonData = JsonUtility.ToJson(_class, true); //.ToJSON();
                }
                if (GUILayout.Button("Import class from Json"))
                {
                    JsonUtility.FromJsonOverwrite(_jsonData, _class); //.FromJSON(_jsonData);
                }
                EditorGUILayout.EndHorizontal();

                _jsonData = EditorGUILayout.TextArea(_jsonData);

                // Save the changes back to the object
                EditorUtility.SetDirty(target);
            }
        }
        
        void OnGUI()
        {
            EditorUtility.SetDirty(target);
        }
        
        private void CreateDirectory(string resource)
        {
            var _class = target as GS.Resource.ResourceFolder;

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets/", "Resources");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/" + _class.path))
            {
                AssetDatabase.CreateFolder("Assets/Resources", _class.path);
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/" + _class.path + "/" + resource))
            {
                AssetDatabase.CreateFolder("Assets/Resources/" + _class.path, resource);
            }
            Debug.Log("Directory to Assets/Resources/" + _class.path + "/" + resource + " should exist now!");
        }

        #region Create new asset

        private int selectedAssetType = 0;
        private string[] assetTypeNames = { };
        private System.Type[] assetTypes = { };
        private string fileName = "New ResourceTable";

        private void FindAssetTypes()
        {
            List<string> _typeNames = new List<string>();
            List<System.Type> _assetTypes = new List<System.Type>();
            System.Type tableType = typeof(GS.Resource.ResourceTable);
            foreach (System.Type type in System.Reflection.Assembly.GetAssembly(tableType).GetTypes().Where(tableType.IsAssignableFrom))
            {
                _assetTypes.Add(type);
                _typeNames.Add(type.Name);
            }
            // Remove GS.Resource.ResourceTable from the list.
            _assetTypes.RemoveAt(0);
            _typeNames.RemoveAt(0);
            assetTypes = _assetTypes.ToArray();
            assetTypeNames = _typeNames.ToArray();
            _assetTypes.Clear();
            _typeNames.Clear();
        }

        private void DrawAssetSelection()
        {
            if (assetTypeNames.Length.Equals(0))
            {
                // Get new asset assetTypes, there should be more!
                FindAssetTypes();
                //Debug.Log("There was no assetTypes");
                if (assetTypeNames.Length.Equals(0))
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("No GS.Resource.ResourceTable inheritances found!");
                    if (GUILayout.Button("Try find again"))
                    {
                        FindAssetTypes();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                selectedAssetType = GUILayout.SelectionGrid(selectedAssetType, assetTypeNames, assetTypeNames.Length);

                EditorGUILayout.BeginHorizontal();
                fileName = EditorGUILayout.TextField(fileName);
                if (GUILayout.Button("Add new ResourceTable asset"))
                {
                    // Add selected asset.
                    AddAsset(fileName);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void AddAsset(string assetName)
        {
            var _class = target as GS.Resource.ResourceFolder;
            
            // If directory is missing, create it.
            CreateDirectory(_class.resource);
            FindAssetTypes();

            // Create asset and set values
            GS.Resource.ResourceTable asset = (GS.Resource.ResourceTable)
                ScriptableObject.CreateInstance(assetTypes[selectedAssetType]);

            string filePath = string.Format(
                "Assets/Resources/{0}/{1}/{2}.asset",
                serializedObject.FindProperty("path").stringValue,
                serializedObject.FindProperty("resource").stringValue,
                assetName
            );

            AssetDatabase.CreateAsset(asset, filePath);
            asset.name = "New ResourceTable";
            asset.updateEvents = new string[] { _class.resourceKey };
            AssetDatabase.SaveAssets();

            // Find the edited property
            SerializedProperty prop = serializedObject.FindProperty("tables");

            // Add new item to array.
            int x = prop.arraySize;
            prop.InsertArrayElementAtIndex(x);
            prop.GetArrayElementAtIndex(x).objectReferenceValue = asset;
            serializedObject.ApplyModifiedProperties();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        #endregion // Create new asset

        private void FindAssets()
        {
            var _class = target as GS.Resource.ResourceFolder;
            
            _class.tables = Resources.LoadAll<GS.Resource.ResourceTable>(
                string.Format("{0}/{1}", _class.path, _class.resource)
            );
            
            //Debug.Log(string.Format("Loaded {0} files\nRequested folder was {1}", _class.tables.Length, "Assets/Resources/" + _class.path + "/" + _class.resource), this);
            
            EditorUtility.SetDirty(target);
            EditorUtility.FocusProjectWindow();
        }
        
        private void CopyAssets()
        {
            // TODO ResourceFolderEditor Copy assets!
            var _class = target as GS.Resource.ResourceFolder;

            // Create new resource type using same ResourceFolder in same resource path.
            GS.Resource.ResourceFolder folder = (GS.Resource.ResourceFolder) ScriptableObject.CreateInstance(typeof(GS.Resource.ResourceFolder));
            folder.path = _class.path;
            folder.resourceKey = _class.resourceKey;
            folder.resource = _resource;

            // Create possibly missing directory.
            CreateDirectory(_resource);

            List<GS.Resource.ResourceTable> _tables = new List<GS.Resource.ResourceTable>();
            foreach(GS.Resource.ResourceTable _table in _class.tables)
            {
                // Create asset and set values
                GS.Resource.ResourceTable asset = (GS.Resource.ResourceTable) ScriptableObject.CreateInstance(_table.GetType());
                asset.name = _table.name;
                asset.updateEvents = _table.updateEvents;
                _tables.Add(asset);

                // Create ResourceTable asset
                AssetDatabase.CreateAsset(asset, string.Format(
                    "Assets/Resources/{0}/{1}/{2}.asset",
                    folder.path, folder.resource,
                    // Get old ResourceTable location, split path and cut extension off it.
                    AssetDatabase.GetAssetPath(_table).Split('/').Last().Split('.')[0]
                ));
            }
            folder.tables = _tables.ToArray();

            // Get old ResourceFolder location and cut extension from it.
            string folderLocation = AssetDatabase.GetAssetPath(_class).Split('.')[0];
            int folderCount = AssetDatabase.FindAssets(
                string.Format("{0} t:GS.Resource.ResourceFolder", folderLocation.Split('/').Last()),
                new[] { folderLocation.Remove(folderLocation.LastIndexOfAny("/".ToCharArray())) }).Length;

            // Create ResourceFolder asset
            AssetDatabase.CreateAsset(folder, string.Format(
                "{0}{1}.asset", folderLocation, folderCount - 1));

            // Finalize all created assets
            AssetDatabase.SaveAssets();
            _tables.Clear();
        }
    }
}
