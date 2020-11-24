using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GS.Data
{
    /// <summary>
    /// Progress manager.
    /// Handles game periodic save progress.
    /// </summary>
    public class ProgressManager : MonoBehaviour
    {
        #region Singleton

        private static ProgressManager manager;
        public static ProgressManager Instance
        {
            get
            {
                //EventManager is not initialized
                if (!manager)
                {
                    manager = FindObjectOfType(typeof(ProgressManager)) as ProgressManager;

                    //No EventManager in scene
                    if (!manager)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene");
                    }
                    else
                    {
                        manager.Init();
                    }
                }
                return manager;
            }
        }
        private void Init()
        {
            if (manager == null)
            {
                manager = new ProgressManager();
            }
        }

        #endregion // Singleton

        #region Values

        public float progressSaveEverySec = 300f;

        [Tooltip("Using Application.presistentDataPath as savefile path.")]
        public const string savePath = "/SaveFiles/";
        public string currentSave = "TestSave";

        private ProgressSave[] progresses = {};

        #endregion // Values

        #region EventTriggers

        public const string GameSaveStartedTrigger = "GameSaveStarted";
        public const string GameSaveEndedTrigger = "GameSaveEnded";
        public const string GameProgressSaveStartedTrigger = "GameProgressSaveStarted";
        public const string GameProgressSaveEndedTrigger = "GameProgressSaveEnded";
        public const string GameTemporarySaveTrigger = "GameTemporarySave";
        public const string GameSaveTrigger = "GameSave";

        public const string GameLoadStartedTrigger = "GameSaveStarted";
        public const string GameLoadEndedTrigger = "GameSaveEnded";
        public const string GameProgressLoadStartedTrigger = "GameProgressSaveStarted";
        public const string GameProgressLoadEndedTrigger = "GameProgressSaveEnded";

        #endregion // EventTriggers

        private float nextSave = 0f;
        // Update is called once per frame
        void Update()
        {
            // Let processes update their stuff.
            foreach (ProgressSave progress in progresses)
            {
                progress.ProcessUpdate();
            }

            if (nextSave < Time.time)
            {
                nextSave = Time.time + progressSaveEverySec;

                if(progresses.Length > 0)
                {
                    GS.Data.EventManager.TriggerEvent(GameSaveStartedTrigger);
                    SaveProgress(currentSave);
                    GS.Data.EventManager.TriggerEvent(GameSaveEndedTrigger);
                }
                else
                {
                    GS.Data.EventManager.TriggerEvent(GameLoadStartedTrigger);
                    LoadLastSave();
                    GS.Data.EventManager.TriggerEvent(GameLoadEndedTrigger);
                }
            }
        }

        public void AddProgress(ProgressSave _progress)
        {
            List<ProgressSave> list = new List<ProgressSave>();
            list.AddRange(progresses);
            list.Add(_progress);
            progresses = list.ToArray();
            list.Clear();
        }

        public string[] GetSaveFiles()
        {
            if (Directory.Exists(Application.persistentDataPath + savePath))
            {
                return Directory.GetFiles(Application.persistentDataPath + savePath, ".save");
            }
            return null;
        }

        private void LoadLastSave()
        {
            if (Directory.Exists(Application.persistentDataPath + savePath))
            {
                string[] files = Directory.GetFiles(Application.persistentDataPath + savePath);

                long latestTick = 0;
                string lastSaveFile = "";
                foreach (string file in files)
                {
                    long ticks = Directory.GetLastWriteTime(Application.persistentDataPath + savePath + file).Ticks;
                    if (latestTick == 0)
                    {
                        latestTick = ticks;
                        lastSaveFile = file;
                    }
                    else
                    {
                        if (ticks < latestTick)
                        {
                            latestTick = ticks;
                            lastSaveFile = file;
                        }
                    }
                }

                //Load progress if exists.
                if(lastSaveFile == "")
                {
                    Debug.Log("No save files to load");
                }
                else
                {
                    Debug.Log("Found file " + lastSaveFile + ", created " + new System.DateTime(latestTick).ToLocalTime().ToString());
                    LoadProgress(lastSaveFile, lastSaveFile.Contains(".temp."));
                }
            }
        }

        /// <summary>
        /// Saves the progress to temporary file.
        /// </summary>
        public void SaveProgress(string _saveFile, bool _temporarySave = true)
        {
            GS.Data.EventManager.TriggerEvent(GameProgressSaveStartedTrigger);

            // Ready progress scripts for save.
            foreach(ProgressSave progress in progresses)
            {
                progress.ProcessForSave();
            }

            if (_temporarySave)
            {
                GS.Data.EventManager.TriggerEvent(GameTemporarySaveTrigger);

                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(Application.persistentDataPath + savePath + _saveFile.Split('.')[0] + ".temp.save", FileMode.Create);
                formatter.Serialize(stream, progresses);
                stream.Close();
            }
            else
            {
                GS.Data.EventManager.TriggerEvent(GameSaveTrigger);

                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(Application.persistentDataPath + savePath + _saveFile.Split('.')[0] + ".save", FileMode.Create);
                formatter.Serialize(stream, progresses);
                stream.Close();
            }

            GS.Data.EventManager.TriggerEvent(GameProgressSaveEndedTrigger);
        }

        /// <summary>
        /// Loads the progress from save file.
        /// </summary>
        public void LoadProgress(string _saveFile, bool _temporarySave)
        {
            GS.Data.EventManager.TriggerEvent(GameProgressLoadStartedTrigger);

            if (_temporarySave)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(Application.persistentDataPath + savePath + _saveFile.Split('.')[0] + ".temp.save", FileMode.Open);
                progresses = formatter.Deserialize(stream) as ProgressSave[];
                stream.Close();
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(Application.persistentDataPath + savePath + _saveFile.Split('.')[0] + ".save", FileMode.Open);
                progresses = formatter.Deserialize(stream) as ProgressSave[];
                stream.Close();
            }

            // Tell progress to handle loading of the save.
            foreach(ProgressSave progress in progresses)
            {
                progress.ProcessAfterLoad();
            }

            GS.Data.EventManager.TriggerEvent(GameProgressLoadEndedTrigger);
        }

        ~ProgressManager()
        {
            if (Instance != this) return;

            SaveProgress(currentSave);
            progresses = null;
        }
    }
}