using UnityEngine;

namespace BaseGame
{
    public class SceneTransitionManager : MonoBehaviour
    {
        // ---- / Singleton / ---- //
        public static SceneTransitionManager Instance;
        
        // ---- / Public Variables / ---- //
        public bool isFadingIn { get; private set; }
        public bool isFadingOut { get; private set; }
        
        // ---- / Private Variables / ---- //
        private Animator _transitionAnimator;

        private void Update()
        {
            if (Input.GetKeyDown("2"))
            {
                EndAnimation();
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            _transitionAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            EndAnimation();
        }

        public void EndLoadIn()
        {
            isFadingIn = false;
            EndAnimation();
        }
        
        public void EndLoadOut()
        {
            isFadingOut = false;
        }

        public void StartAnimation()
        {
            _transitionAnimator.SetTrigger("triggerStart");
            isFadingIn = true;
        }

        private void EndAnimation()
        {
            _transitionAnimator.SetTrigger("triggerEnd");
            isFadingOut = true;
        }
    }
}