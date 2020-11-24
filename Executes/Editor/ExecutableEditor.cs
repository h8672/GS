using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GS.Executes.Editors
{
    //[CanEditMultipleObjects]
    [CustomEditor(typeof(GS.Executes.Executable), true)]
    public class ExecutableEditor : UnityEditor.Editor
    {
        private Color rayColor = new Color(1f, 0f, 0f);
        private Vector3 rayOffset = Vector3.up;
        private GS.Executes.Executor[] executors = { };

        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            rayColor = EditorGUILayout.ColorField(rayColor);
            rayOffset = EditorGUILayout.Vector3Field("Target offset", rayOffset);
            if ( GUILayout.Button("Find executors with ray") )
            {
                var _class = target as GS.Executes.Executable;

                if ( executors.Length.Equals(0) )
                {
                    FindExecutors();
                    if ( executors.Length.Equals(0) ) {
                        EditorGUILayout.HelpBox("No Executors containing executable found!", MessageType.Error);
                    }
                }

                Debug.Log("Executors length: " + executors.Length);
                for (int i = 0; i < executors.Length; i++)
                {
                    DrawRayToPoint(_class.transform.position, executors[i].transform.position, rayOffset);
                }
            }
        }

        void OnGUI()
        {

        }

        // Find all Executors from current scene and draw line to those.
        private void FindExecutors()
        {
            var _class = target as GS.Executes.Executable;

            List<GS.Executes.Executor> _executors = new List<GS.Executes.Executor>();
            _executors.AddRange(FindObjectsOfType<GS.Executes.Executor>());

            bool deleteOption;
            for (int i = 0; i < _executors.Count; i++)
            {
                do
                {
                    if (_executors.Count > i && !_executors[i].ContainsExecutable(_class))
                    {
                        _executors.RemoveAt(i);
                        deleteOption = true;
                    }
                    else { deleteOption = false; }
                } while (deleteOption);
            }
            executors = _executors.ToArray();
            Debug.Log("Executors count: " + _executors.Count);
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