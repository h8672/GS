using UnityEngine;
using UnityEditor;

namespace GS.Editor
{
    public static class TopMenu
    {
        #region AddGameObject methods

        /// <summary>
        /// Adds the game object.
        /// </summary>
        /// <param name="_obj">Object.</param>
        private static void AddGameObject(out GameObject _obj)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(Selection.activeTransform);
            Selection.SetActiveObjectWithContext(obj, Selection.activeContext);
            _obj = obj;
        }

        /// <summary>
        /// Adds the primitive game object.
        /// </summary>
        /// <param name="_type">Type.</param>
        /// <param name="_obj">Object.</param>
        private static void AddPrimitiveGameObject(PrimitiveType _type, out GameObject _obj)
        {
            GameObject obj = GameObject.CreatePrimitive(_type);
            obj.transform.SetParent(Selection.activeTransform);
            Selection.SetActiveObjectWithContext(obj, Selection.activeContext);
            _obj = obj;
        }

        private static void AddPrimitiveWithObjects(string _name, PrimitiveType _type, out GameObject _obj, params System.Type[] components)
        {
            AddPrimitiveGameObject(_type, out GameObject obj);
            obj.name = _name;

            foreach(System.Type component in components)
            {
                obj.AddComponent(component);
            }
            _obj = obj;
        }

        /// <summary>
        /// Adds the game object with components.
        /// </summary>
        /// <param name="requiresCanvas">If set to <c>true</c> requires canvas.</param>
        /// <param name="_obj">Object.</param>
        /// <param name="components">Components.</param>
        private static void AddGameObjectWithComponents(string _name, bool requiresCanvas, out GameObject _obj, params System.Type[] components)
        {
            AddGameObject(out GameObject obj);
            obj.name = _name;
            if (requiresCanvas)
            {
                if( obj.GetComponentInParent<Canvas>() == null )
                {
                    // So brilliant resolution!
                    AddGameObjectWithComponents(
                        "Canvas", false,
                        out GameObject _canvas,
                        typeof(Canvas),
                        typeof(UnityEngine.UI.CanvasScaler),
                        typeof(UnityEngine.UI.GraphicRaycaster)
                    );

                    // Set Canvas values
                    _canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                    _canvas.transform.SetParent(null);

                    // Set object parent
                    obj.transform.SetParent(_canvas.transform);
                    obj.transform.localPosition = Vector3.zero;
                }
            }

            foreach(System.Type component in components)
            {
                obj.AddComponent(component);
            }
            _obj = obj;
        }

        /// <summary>
        /// Adds the game object with components.
        /// Never requires Canvas.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_obj">Object.</param>
        /// <param name="components">Components.</param>
        private static void AddGameObjectWithComponents(string _name, out GameObject _obj, params System.Type[] components)
        {
            AddGameObjectWithComponents(_name, false, out GameObject obj, components);
            _obj = obj;
        }

        /// <summary>
        /// Adds the game object with components.
        /// Doesn't expose GameObject.
        /// </summary>
        /// <param name="requiresCanvas">If set to <c>true</c> requires canvas.</param>
        /// <param name="components">Components.</param>
        private static void AddGameObjectWithComponents(string _name, bool requiresCanvas, params System.Type[] components)
        {
            AddGameObjectWithComponents(_name, requiresCanvas, out GameObject obj, components);
        }

        /// <summary>
        /// Adds the game object with components.
        /// Doesn't expose GameObject.
        /// Never requires Canvas.
        /// </summary>
        /// <param name="components">Components.</param>
        private static void AddGameObjectWithComponents(string _name, params System.Type[] components)
        {
            AddGameObjectWithComponents(_name, false, out GameObject obj, components);
        }

        #endregion // AddGameObject methods
        #region Add GameObjects

        #region Create fillers

        [MenuItem("GameObject/GS/3D/Language/AudioFiller")]
        private static void AddAudioFiller()
        {
            AddGameObjectWithComponents(
                "AudioFiller", false,
                out GameObject obj,
                typeof(GS.Resource.Filler.AudioFiller)
            );
        }

        [MenuItem("GameObject/GS/UI/Language/ImageFiller")]
        private static void AddImageFiller()
        {
            AddGameObjectWithComponents(
                "ImageFiller", false,
                out GameObject obj,
                typeof(GS.Resource.Filler.ImageFiller)
            );
        }

        [MenuItem("GameObject/GS/UI/Language/TextFiller")]
        private static void AddTextFillerUI() {
            AddGameObjectWithComponents(
                "TextFillerUI", true,
                out GameObject obj,
                typeof(GS.Resource.Filler.TextFillerUI)
            );
            obj.GetComponent<TMPro.TextMeshProUGUI>().text = "New TextUI";
        }

        [MenuItem("GameObject/GS/3D/Language/TextFiller")]
        private static void AddTextFiller3D() {
            AddGameObjectWithComponents(
                "TextFiller3D", false,
                out GameObject obj,
                typeof(GS.Resource.Filler.TextFiller3D)
            );
            obj.GetComponent<TMPro.TextMeshPro>().text = "New Text3D";
        }

        #endregion // Create fillers
        #region Create Executors

        [MenuItem("GameObject/GS/3D/Executor/ExecuteOnTagStay")]
        private static void AddExecuteOnTagStay()
        {
            AddPrimitiveWithObjects(
                "Cube - ExecuteOnTagStay",
                PrimitiveType.Cube,
                out GameObject obj,
                //typeof(BoxCollider),
                typeof(GS.Executes.Executors.ExecuteOnTagStay)
            );
        }

        #endregion // Create Executors
        #region RPG elements

        [MenuItem("GameObject/GS/Controls/InteractionComponent", priority = 12)]
        private static void AddInteractionComponent()
        {
            if (Selection.gameObjects.Length > 0)
            {
                Selection.gameObjects[0].AddComponent<GS.Controls.Interaction>();
                LayerMask.GetMask(LayerMask.LayerToName(Selection.gameObjects[0].layer));
                Selection.gameObjects[0].layer += LayerMask.GetMask("Interactable");
            }
        }

        #endregion // RPG elements

        #endregion // Add Gameobjects
    }
}