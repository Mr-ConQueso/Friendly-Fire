using UnityEngine;
using System.Collections;

namespace Gameplay.Helper
{
    public class HoverableObject : MonoBehaviour, IHoverable
    {
        // ---- / Serialized Variables / ---- //
        [SerializeField] protected float lerpDuration = 0.5f;
        [SerializeField] protected Vector3 endPosition = Vector3.zero;
        [SerializeField] protected float endScale = 1.0f;
        
        // ---- / Protected Variables / ---- //
        [HideInInspector] protected bool IsInteractable = true;
        [HideInInspector] protected Vector3 StartPosition;
        [HideInInspector] protected Vector3 StartScale;
        [HideInInspector] protected Coroutine CurrentCoroutine;
        
        protected virtual void Start()
        {
            StartPosition = transform.position;
            StartScale = transform.localScale;
        }
    
        public virtual void OnMouseEnter()
        {
            if (!IsInteractable) return;
            if (CurrentCoroutine != null) StopCoroutine(CurrentCoroutine);
            
            CurrentCoroutine = StartCoroutine(LerpTo(StartPosition + endPosition, StartScale * endScale));
        }

        public virtual void OnMouseOver() {}

        public virtual void OnMouseExit()
        {
            if (!IsInteractable) return;
            if (CurrentCoroutine != null) StopCoroutine(CurrentCoroutine);
            
            CurrentCoroutine = StartCoroutine(LerpTo(StartPosition, StartScale));
        }
        
        protected void DisableInteractable()
        {
            IsInteractable = false;
            StartCoroutine(ReverseToStart());
        }

        protected IEnumerator LerpTo(Vector3 targetPosition, Vector3 targetScale)
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
            if (CurrentCoroutine != null)
            {
                StopCoroutine(CurrentCoroutine);
            }
            CurrentCoroutine = StartCoroutine(LerpTo(StartPosition, StartScale));
            yield return CurrentCoroutine;
            CurrentCoroutine = null;
        }
    }
}