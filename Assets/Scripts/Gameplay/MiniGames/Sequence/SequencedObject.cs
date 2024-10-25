using Gameplay.Helper;
using UnityEngine;

namespace Gameplay.MiniGames.HiddenBalls
{
    public class SequencedObject : MonoBehaviour, IHoverable
    {
        // ---- / Public Variables / ---- //
        [HideInInspector] public int ObjectIndex;
        
        // ---- / Private Variables / ---- //
        private bool _isInteractable = true;
        private bool _isActive = false;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            SequenceController.OnResetSequencedObjects += OnResetSequencedObjects;
            SequenceController.OnSetSequencedObjectInteractable += OnSetSequencedObjectInteractable;
        }

        private void OnDestroy()
        {
            SequenceController.OnResetSequencedObjects -= OnResetSequencedObjects;
            SequenceController.OnSetSequencedObjectInteractable -= OnSetSequencedObjectInteractable;
        }

        private void OnMouseDown()
        {
            if (_isInteractable) 
            {
                _isActive = !_isActive;
                if (_isActive)
                {
                    ActivateColor();
                    SequenceController.Instance.ClickSequencedObject(ObjectIndex);
                }
                else
                {
                    DeactivateColor();
                    SequenceController.Instance.ClearSequence();
                }
                DisableInteractable();
            }
        }

        public void DeactivateColor()
        {
            _meshRenderer.material.color = Color.white;
        }

        public void ActivateColor()
        {
            _meshRenderer.material.color = Color.red;
        }

        public void OnMouseEnter()
        {
            if (_isInteractable)
            {
            }
        }

        public void OnMouseOver()
        {
            if (_isInteractable)
            {
            }
        }

        public void OnMouseExit()
        {
            if (_isInteractable)
            {
            }
        }
        
        private void OnResetSequencedObjects(bool willActivate)
        {
            if (willActivate)
            {
                _isActive = true;
                ActivateColor();
            }
            else
            {
                _isActive = false;
                DeactivateColor();
            }
        }
        
        private void OnSetSequencedObjectInteractable(bool willInteractable)
        {
            if (willInteractable)
            {
                _isInteractable = true;
            }
            else
            {
                DisableInteractable();
            }
        }
        
        public void DisableInteractable()
        {
            _isInteractable = false;
        }
    }
}