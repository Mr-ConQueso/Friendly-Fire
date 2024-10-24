using UnityEngine;
using System.Collections;

namespace Gameplay.Helper
{
    public class HoverableObject : MonoBehaviour, IHoverable
    {
        // ---- / Protected Variables / ---- //
        [HideInInspector] protected bool IsInteractable = true;
        
        // ---- / Serialized Variables / ---- //
        [SerializeField] private float lerpDuration = 0.5f;
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private float endScale;
    
        // ---- / Private Variables / ---- //
        private Vector3 _startPosition;
        private Vector3 _startScale;
        private Coroutine _currentCoroutine;

        protected virtual void Start()
        {
            _startPosition = transform.position;
            _startScale = transform.localScale;
        }
    
        public void OnMouseEnter()
        {
            if (IsInteractable)
            {
                if (_currentCoroutine != null)
                {
                    StopCoroutine(_currentCoroutine);
                }
                _currentCoroutine = StartCoroutine(LerpTo(_startPosition + endPosition, _startScale * endScale));
            }
        }

        public void OnMouseOver() {}

        public void OnMouseExit()
        {
            if (IsInteractable)
            {
                if (_currentCoroutine != null)
                {
                    StopCoroutine(_currentCoroutine);
                }
                _currentCoroutine = StartCoroutine(LerpTo(_startPosition, _startScale));
            }
        }
        
        protected void DisableInteractable()
        {
            IsInteractable = false;
            StartCoroutine(ReverseToStart());
        }

        private IEnumerator LerpTo(Vector3 targetPosition, Vector3 targetScale)
        {
            float elapsedTime = 0f;
            Vector3 initialPosition = transform.position;
            Vector3 initialScale = transform.localScale;

            while (elapsedTime < lerpDuration)
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / lerpDuration);
                transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / lerpDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
            transform.localScale = targetScale;
        }

        private IEnumerator ReverseToStart()
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            _currentCoroutine = StartCoroutine(LerpTo(_startPosition, _startScale));
            yield return _currentCoroutine;
            _currentCoroutine = null;
        }
    }
}