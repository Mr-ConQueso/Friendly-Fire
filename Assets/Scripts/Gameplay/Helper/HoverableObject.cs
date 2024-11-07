using UnityEngine;
using System.Collections;

namespace Gameplay.Helper
{
    public class HoverableObject : MonoBehaviour, IHoverable
    {
        // ---- / Serialized Variables / ---- //
        [SerializeField] protected float LerpDuration = 0.5f;
        [SerializeField] protected Vector3 EndPosition = Vector3.zero;
        [SerializeField] protected float EndScale = 1.0f;
        
        // ---- / Protected Variables / ---- //
        protected bool isInteractable { get; private set; } = true;
        protected Vector3 StartPosition;
        protected Vector3 StartScale;
        
        // ---- / Private Variables / ---- //
        private Coroutine _currentCoroutine;
        
        protected virtual void Start()
        {
            StartPosition = transform.position;
            StartScale = transform.localScale;
        }
    
        public virtual void OnMouseEnter()
        {
            if (!isInteractable) return;
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(LerpTo(StartPosition + EndPosition, StartScale * EndScale));
        }

        public virtual void OnMouseOver() {}

        public virtual void OnMouseExit()
        {
            if (!isInteractable) return;
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(LerpTo(StartPosition, StartScale));
        }
        
        protected void DisableInteractable()
        {
            isInteractable = false;
            StartCoroutine(ReverseToStart());
        }
        
        protected void EnableInteractable()
        {
            isInteractable = true;
        }

        protected IEnumerator LerpTo(Vector3 targetPosition, Vector3 targetScale)
        {
            float elapsedTime = 0f;
            Vector3 initialPosition = transform.position;
            Vector3 initialScale = transform.localScale;

            while (elapsedTime < LerpDuration)
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / LerpDuration);
                transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / LerpDuration);
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
            _currentCoroutine = StartCoroutine(LerpTo(StartPosition, StartScale));
            yield return _currentCoroutine;
            _currentCoroutine = null;
        }
    }
}