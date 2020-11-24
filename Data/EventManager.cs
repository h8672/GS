using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* This EventManager is based on a Unitys tutorial, but I added some of my own ideas.
 * https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
 * - UnityEvent works in Game and GUI, maybe in other platforms aswell and save some compile time?
 * 
 * There is also information learned about Unitys current InputManager which
 * was the reason I started to make my own InputManager.
 */

namespace GS.Data
{
    public class EventManager : MonoBehaviour
    {
        #region Singleton

        private static EventManager eventManager;
        public static EventManager Instance
        {
            get
            {
                //EventManager is not initialized
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    //No EventManager in scene
                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene");
                    }
                    else
                    {
                        //Initialize EventManager
                        eventManager.Init();
                    }
                }
                //All good!
                return eventManager;
            }
        }
        private void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, UnityEvent>();
            }
        }

        #endregion // Singleton

        private Dictionary<string, UnityEvent> eventDictionary;

        //Create an eventlistener and add function to it.
        public static void StartListening(string eventName, UnityAction listener)
        {
            if (Instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        //Stop listening this event.
        public static void StopListening(string eventName, UnityAction listener)
        {
            if (eventManager == null)
                return;
            if (Instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
                thisEvent.RemoveListener(listener);
        }

        //Call UnityAction.
        public static void TriggerEvent(string eventName)
        {
            if (Instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
                thisEvent.Invoke();
        }
    }
}