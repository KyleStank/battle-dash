using UnityEngine;

namespace TurmoilStudios.Utils {
    /// <summary>
    /// Singleton pattern class.
    /// </summary>
    /// <typeparam name="T">The generic type of the class. Must be a Component..</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Component {
        protected static T _instance;

        /// <summary>
        /// Instance of the class.
        /// </summary>
        public static T Instance {
            get {
                if(_instance == null) {
                    _instance = FindObjectOfType<T>();
                    if(_instance == null) {
                        GameObject obj = new GameObject();
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Initialize our instance here. If you need to use Awake, make sure to invoke base.Awake() first.
        /// </summary>
        protected virtual void Awake() {
            _instance = this as T;
        }
    }
}
