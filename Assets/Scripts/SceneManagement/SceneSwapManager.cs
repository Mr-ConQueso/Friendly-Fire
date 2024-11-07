using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseGame
{
    public class SceneSwapManager : MonoBehaviour
    {
        // ---- / Singleton / ---- //
        private static SceneSwapManager _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            //SceneTransitionManager.Instance.StartAnimation();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public static void SwapScene(string scene)
        {
            _instance.StartCoroutine(TransitionTheSwapScene(scene));
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            StopAllCoroutines();
        }

        private static IEnumerator TransitionTheSwapScene(string scene)
        {
            SceneTransitionManager.Instance.StartAnimation();

            while (SceneTransitionManager.Instance.isFadingIn)
            {
                yield return null;
            }

            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}