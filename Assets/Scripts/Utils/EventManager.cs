using System.Collections.Generic;
using UnityEngine.Events;

namespace TurmoilStudios.Utils {
    /// <summary>
    /// Handles all of the events used in the game.
    /// </summary>
    public static class EventManager {
        static Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();

        #region Methods

        #region Public methods
        /// <summary>
        /// Makes a listener start listening for an event.
        /// </summary>
        /// <param name="eventName">Name of event.</param>
        /// <param name="listener">Actual method that will be fired.</param>
        public static void StartListening(string eventName, UnityAction listener) {
            UnityEvent thisEvent = null;

            //Check if event key exists
            if(eventDictionary.TryGetValue(eventName, out thisEvent)) { //If we do, add event to it
                thisEvent.AddListener(listener);
            } else { //If not, create new event in dictionary, then add event to it
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Makes a listener stop listening for an event.
        /// </summary>
        /// <param name="eventName">Name of event to remove.</param>
        /// <param name="listener">Method of event that will be removed.</param>
        public static void StopListening(string eventName, UnityAction listener) {
            UnityEvent thisEvent = null;
            if(eventDictionary.TryGetValue(eventName, out thisEvent))
                thisEvent.RemoveListener(listener);
        }

        /// <summary>
        /// Removes all listeners from an event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        public static void RemoveListenersFrom(string eventName) {
            if(eventDictionary.ContainsKey(eventName))
                eventDictionary[eventName].RemoveAllListeners();
        }

        public static int len() {
            return eventDictionary.Count;
        }

        /// <summary>
        /// Fires an event.
        /// </summary>
        /// <param name="eventName">Name of event to fire.</param>
        public static void TriggerEvent(string eventName) {
            UnityEvent thisEvent = null;

            if(eventDictionary.TryGetValue(eventName, out thisEvent))
                thisEvent.Invoke();
        }
        #endregion

        #endregion
    }
}
