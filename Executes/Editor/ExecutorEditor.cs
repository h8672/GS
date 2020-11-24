using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GS.Executes.Editors
{
    //[CanEditMultipleObjects]
    [CustomEditor(typeof(GS.Executes.Executor), true)]
    public class ExecutorEditor : UnityEditor.Editor
    {
        private Color rayColor = Color.red;
        private Vector3 rayOffset = Vector3.up;

        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            rayColor = EditorGUILayout.ColorField(rayColor);
            rayOffset = EditorGUILayout.Vector3Field("Ray offset", rayOffset);
            if ( GUILayout.Button("Find executables with ray") )
            {
                SerializedProperty array = serializedObject.FindProperty("executables");
                var _class = target as GS.Executes.Executor;
                Executable exe = null;
                for (int i = 0; i < array.arraySize; i++)
                {
                    exe = array.GetArrayElementAtIndex(i).objectReferenceValue as GS.Executes.Executable;
                    if (exe == null) continue;
                    DrawRayToPoint(_class.gameObject.transform.position, exe.gameObject.transform.position, rayOffset);
                }
            }
        }

        void OnGUI()
        {

        }

        private void DrawRayToPoint(Vector3 _pointA, Vector3 _pointB, Vector3 _offset)
        {
            Debug.LogWarning("If you don't see drawn ray, check if scene has gizmos on?");
            UnityEngine.Debug.DrawRay(_pointA, _offset, rayColor);
            UnityEngine.Debug.DrawRay(_pointA + _offset, _pointB - _pointA, rayColor);
            UnityEngine.Debug.DrawRay(_pointB + _offset, -_offset, rayColor);
        }
    }
}