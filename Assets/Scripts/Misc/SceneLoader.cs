using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TurmoilStudios.BattleDash {
    public sealed class SceneLoader : MonoBehaviour {
        [SerializeField]
        string sceneName = "";

        #region Methods

        #region Unity methods
        void Start() {
            StartCoroutine(Load());
        }
        #endregion

        #region IEnumerator methods
        IEnumerator Load() {
            //Load scene
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while(!async.isDone)
                yield return null;

            //Unload current scene
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }
        #endregion

        #endregion
    }
}
