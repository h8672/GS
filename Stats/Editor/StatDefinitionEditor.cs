using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GS.Stats
{
    /// <summary>
    /// Stat definition editor.
    /// Added simple curve editor if want to change curve with float values at specific keyframes.
    /// Took way too long to implement.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GS.Stats.StatDefinition), true)]
    public class StatDefinitionEditor : UnityEditor.Editor
    {
        int _targetKey = 0;
        int _selectedKey = 0;
        Vector2 _vectorPoint = Vector2.zero;
        bool _changed = false;

        private void LoadPoint(AnimationCurve _curve)
        {
            _vectorPoint = new Vector2(
                _curve.keys[_selectedKey].time,
                _curve.keys[_selectedKey].value
            );
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SerializedProperty _curveProperty = serializedObject.FindProperty("efficiencyCurve");

            AnimationCurve _curve = _curveProperty.animationCurveValue;

            if (_curve.keys.Length < 2)
            {
                if (GUILayout.Button("Reset curve, no KeyFrames"))
                {
                    List<Keyframe> keys = new List<Keyframe>();
                    keys.Add(new Keyframe(0f, 0f, 0f, 0f, 0f, 0f));
                    keys.Add(new Keyframe(1f, 1f, 0f, 0f, 0f, 0f));
                    _curve.keys = keys.ToArray();
                    _curveProperty.animationCurveValue = _curve;
                    _selectedKey = 0;
                    LoadPoint(_curve);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(this);
                }
                return;
            }

            // Get Keyframe! Else, there has been change!
            if (_vectorPoint == null) { LoadPoint(_curve); }
            else { _changed = true; }

            GUILayout.Label("\nCustom curve editor", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            // Change Keyframe values...
            _vectorPoint = EditorGUILayout.Vector2Field("Keyframe position", _vectorPoint);

            if (GUILayout.Button("Save"))
            {
                // Moving the Keyframe with MoveKey function to same index to save it.
                _selectedKey = _curve.MoveKey(_selectedKey, new Keyframe(_vectorPoint.x, _vectorPoint.y));
                _curveProperty.animationCurveValue = _curve;

                EditorUtility.SetDirty(this);
            }

            EditorGUILayout.EndHorizontal();

            // Change Keyframe.
            _targetKey = EditorGUILayout.IntSlider("Selected Keyframe", _selectedKey, 0,  _curve.length - 1);
            if (_targetKey != _selectedKey)
            {
                _changed = false;
                _selectedKey = _targetKey;
                LoadPoint(_curve);
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.HelpBox(
                (!_changed ? "No changes" : "Changed"),
                (!_changed ? MessageType.Info : MessageType.Warning)
            );

            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("New key"))
            {
                // Add key between current and next key, or before last key.
                if (_selectedKey < _curve.length - 1)
                {
                    _vectorPoint.x += (_curve.keys[_selectedKey + 1].time - _curve.keys[_selectedKey].time) / 2;
                    _selectedKey = _curve.AddKey(_vectorPoint.x, _vectorPoint.y);
                }
                else
                {
                    _vectorPoint.x += (_curve.keys[_selectedKey - 1].time - _curve.keys[_selectedKey].time) / 2;
                    _selectedKey = _curve.AddKey(_vectorPoint.x, _vectorPoint.y) + 1;
                }

                _curveProperty.animationCurveValue = _curve;

                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Remove key"))
            {
                _curve.RemoveKey(_selectedKey);
                if(_selectedKey > 0) _selectedKey--;

                _curveProperty.animationCurveValue = _curve;
                LoadPoint(_curve);

                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(this);
        }

        void OnGUI()
        {
            EditorUtility.SetDirty(target);
        }
    }
}