//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace GS.Data
{
    public class Settings : MonoBehaviour
    {
        #region Singleton

        private static Settings settings;
        public static Settings Instance
        {
            get
            {
                //EventManager is not initialized
                if (!settings)
                {
                    settings = FindObjectOfType(typeof(Settings)) as Settings;

                    //No EventManager in scene
                    if (!settings)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene");
                    }
                    else
                    {
                        //Initialize EventManager
                        settings.Init();
                    }
                }
                //All good!
                return settings;
            }
        }
        private void Init()
        {
            if (settings == null)
            {
                settings = new Settings();
            }
        }

        #endregion // Singleton

        #region Language key values

        public GS.Data.Values.SerializedKeyValues keyValues = new GS.Data.Values.SerializedKeyValues();

        #endregion // Language key values

        #region Mouse lock on screen change

        
        public bool inMenu = false;
        public GS.Data.Values.InputMode inputMode = GS.Data.Values.InputMode.MouseAndKeyboard;

        public void CheckInputMode()
        {
            switch (inputMode)
            {
                case Values.InputMode.MouseAndKeyboard:
                    CheckCursorState();
                    break;
                case Values.InputMode.Controller:
                    //Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = false;
                    break;
                default:
                    Debug.LogError("No input mode selected for the game!");
                    break;
            }
        }

        public void CheckCursorState()
        {
            // Check cursor mode based on window mode and share the result with things that require it.
            switch (Screen.fullScreenMode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                    //Fullscreen mode.
                    Cursor.lockState = (inMenu ? CursorLockMode.Confined : CursorLockMode.Locked);
                    break;
                case FullScreenMode.FullScreenWindow:
                    //Borderless window mode.
                    Cursor.lockState = (inMenu ? CursorLockMode.None : CursorLockMode.Locked);
                    break;
                default:
                    //Maximized window, windowed and other modes.
                    Cursor.lockState = (inMenu ? CursorLockMode.None : CursorLockMode.Locked);
                    break;
            }
        }

        #endregion // Mouse lock on screen change

        #region Unity Updates

        void Awake()
        {
            CheckInputMode();
        }

        #if UNITY_EDITOR
        
        void Update()
        {
            if ( Input.GetKey(KeyCode.Escape) )
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }

        #endif

        #endregion // Unity Updates
    }
}
