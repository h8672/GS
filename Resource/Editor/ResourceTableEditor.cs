using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GS.Editor.Resource
{
    //[CanEditMultipleObjects]
    [CustomEditor(typeof(GS.Resource.ResourceTable), true)]
    public class ResourceTableEditor : UnityEditor.Editor
    {
        private string _event = "New update event";
        private string _tag = "New tag";
        private object _object;
        private string _jsonData = "";
        private bool fold;

        public override void OnInspectorGUI()
        {
            //EditorGUILayout.HelpBox("Name is used to", MessageType.Info, true);

            // Draw the default inspector
            DrawDefaultInspector();

            EditorGUILayout.Separator();
            if(GUILayout.Button("Quick edit tools")) { fold = !fold; }
            if (fold)
            {

                #region Quick add data

                EditorGUILayout.Separator();

                #region Trigger event quick input

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Event name");
                _event = EditorGUILayout.TextField(_event);
                if (GUILayout.Button("Add Event"))
                {
                    SerializedProperty prop = serializedObject.FindProperty("updateEvents");
                    int x = prop.arraySize;
                    prop.InsertArrayElementAtIndex(x);
                    prop.GetArrayElementAtIndex(x).stringValue = _event;
                    serializedObject.ApplyModifiedProperties();
                }
                EditorGUILayout.EndHorizontal();

                #endregion // Trigger event quick input

                EditorGUILayout.Separator();
                var _class = target as GS.Resource.ResourceTable;
                System.Type type = _class.GetDataType();

                #region Serialized data draw

                if (type.IsSerializable)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Table tag");
                    _tag = EditorGUILayout.TextArea(_tag);
                    EditorGUILayout.EndHorizontal();

                    // TODO Fix serialized object to be editable... Type is correct, why cant I edit it?.
                    //EditorGUILayout.ObjectField(serializedObject.FindProperty("serializedData"));
                    //tempProp.stringValue = EditorGUILayout.TextField(serializedObject.FindProperty("serializedData").stringValue);

                    // Text values are the same...
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serializedData"), true);
                    //serializedObject.FindProperty("serializedData").stringValue = EditorGUILayout.TextField(serializedObject.FindProperty("serializedData").stringValue);
                    EditorGUILayout.EndVertical();

                    serializedObject.ApplyModifiedProperties();
                    if (GUILayout.Button("Add\nData"))
                    {
                        AddItem();
                    }
                    serializedObject.ApplyModifiedProperties();
                    EditorGUILayout.EndHorizontal();
                }

                #endregion // Serialized data draw
                #region Object data draw

                else
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Tag");
                    _tag = EditorGUILayout.TextArea(_tag);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Object data");
                    _object = EditorGUILayout.ObjectField((Object)_object, type, false);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.EndVertical();

                    if (GUILayout.Button("Add\nData"))
                    {
                        AddItem();
                    }
                    serializedObject.ApplyModifiedProperties();
                    EditorGUILayout.EndHorizontal();
                }

                #endregion // Object data draw
                #region JSON tools

                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Export class to Json"))
                {
                    _jsonData = _class.ToJSON();
                }
                if (GUILayout.Button("Import class from Json"))
                {
                    _class.FromJSON(_jsonData);
                }
                EditorGUILayout.EndHorizontal();
                _jsonData = EditorGUILayout.TextArea(_jsonData);

                #endregion // JSON tools

                #endregion // Quick add data
            }

            // Save the changes back to the object
            EditorUtility.SetDirty(target);
        }

        private void AddItem()
        {
            var _class = target as GS.Resource.ResourceTable;
            // Find the edited property
            SerializedProperty prop = serializedObject.FindProperty("objects");

            // Add new item to array.
            int x = prop.arraySize;
            prop.InsertArrayElementAtIndex(x);
            prop = prop.GetArrayElementAtIndex(x);

            // Set values to new array item.
            prop.FindPropertyRelative("tag").stringValue = _tag;
            if (_class.GetDataType().IsSerializable)
            {
                SerializedProperty data = prop.FindPropertyRelative("data");
                SerializedProperty _data = serializedObject.FindProperty("serializedData");

                #region Data switch case
                switch (_data.propertyType)
                {
                    case SerializedPropertyType.AnimationCurve:
                        data.animationCurveValue = _data.animationCurveValue;
                        break;
                    case SerializedPropertyType.Boolean:
                        data.boolValue = _data.boolValue;
                        break;
                    case SerializedPropertyType.Bounds:
                        data.boundsValue = _data.boundsValue;
                        break;
                    case SerializedPropertyType.Color:
                        data.colorValue = _data.colorValue;
                        break;
                    case SerializedPropertyType.Enum:
                        data.enumValueIndex = _data.enumValueIndex;
                        break;
                    case SerializedPropertyType.ExposedReference:
                        data.exposedReferenceValue = _data.exposedReferenceValue;
                        break;
                    case SerializedPropertyType.String:
                        data.stringValue = _data.stringValue;
                        break;
                    case SerializedPropertyType.Float:
                        data.floatValue = _data.floatValue;
                        break;
                    case SerializedPropertyType.Integer:
                        data.intValue = _data.intValue;
                        break;
                    case SerializedPropertyType.ObjectReference:
                        data.objectReferenceValue = _data.objectReferenceValue;
                        break;
                    case SerializedPropertyType.Quaternion:
                        data.quaternionValue = _data.quaternionValue;
                        break;
                    case SerializedPropertyType.Rect:
                        data.rectValue = _data.rectValue;
                        break;
                    case SerializedPropertyType.Vector2:
                        data.vector2Value = _data.vector2Value;
                        break;
                    case SerializedPropertyType.Vector3:
                        data.vector3Value = _data.vector3Value;
                        break;
                    case SerializedPropertyType.Vector4:
                        data.vector4Value = _data.vector4Value;
                        break;
                    default:
                        Debug.LogWarning("Serialized type isn't supported. Customize?", this);
                        break;
                }
                #endregion // Data switch case
            }
            else
            {
                prop.FindPropertyRelative("data").objectReferenceValue = (Object) _object;
            }
            serializedObject.ApplyModifiedProperties();
        }

        void OnGUI()
        {
            EditorUtility.SetDirty(target);
        }
    }
}
