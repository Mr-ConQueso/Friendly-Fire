using System.Collections;
using Gameplay.Helper;
using UnityEngine;

namespace Gameplay.MiniGames.HiddenBalls
{
    public class SequencedObject : MonoBehaviour, IHoverable
    {
        // ---- / Public Variables / ---- //
        [HideInInspector] public int ObjectIndex;
        
        // ---- / Serialized Variables / ---- //
        [SerializeField] private SoundData _clickSound; 
        
        // ---- / Private Variables / ---- //
        private bool _isInteractable = true;
        private bool _isActive;
        private MeshRenderer _meshRenderer;
        private Light _light;
        private Material _unActiveMaterial, _activeMaterial;
        private int _buttonMaterialIndex;
        
        public void DeactivateButton()
        {
            SetMeshMaterial(_unActiveMaterial);
            _light.enabled = false;
            _isActive = false;
        }

        public void ActivateButton()
        {
            SetMeshMaterial(_activeMaterial);
            _light.enabled = true;
            _isActive = true;
        }

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _light = GetComponentInChildren<Light>();
            SequenceController.OnResetSequencedObjects += OnResetSequencedObjects;
            SequenceController.OnSetSequencedObjectInteractable += OnSetSequencedObjectInteractable;
            SequenceController.OnSetSequencedObjectColor += SetActiveColor;
        }

        private void Start()
        {
            _buttonMaterialIndex = GetButtonMaterialIndex();
            _unActiveMaterial = _meshRenderer.materials[_buttonMaterialIndex];
            _activeMaterial = GetActiveColor(_unActiveMaterial);
            _light.color = _activeMaterial.color;
        }

        private void OnDestroy()
        {
            SequenceController.OnResetSequencedObjects -= OnResetSequencedObjects;
            SequenceController.OnSetSequencedObjectInteractable -= OnSetSequencedObjectInteractable;
            SequenceController.OnSetSequencedObjectColor -= SetActiveColor;
        }
        
        private void DisableInteractable()
        {
            _isInteractable = false;
        }

        private void OnMouseDown()
        {
            if (!_isInteractable || _isActive) return;
            
            ActivateButton();
            SequenceController.Instance.ClickSequencedObject(ObjectIndex);
            Invoke(nameof(DeactivateButton), SequenceController.Instance.TimeBetweenEvents);
            
            AudioController.Instance.CreateSound()
                .WithSoundData(_clickSound)
                .WithRandomPitch(true)
                .WithPosition(this.transform.position)
                .Play();
        }

        public void OnMouseEnter() { }

        public void OnMouseOver() { }

        public void OnMouseExit() { }
        
        private void OnResetSequencedObjects(bool willActivate)
        {
            if (willActivate)
            {
                ActivateButton();
            }
            else
            {
                DeactivateButton();
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
        
        private void SetActiveColor(Color targetColor, bool fadeIn)
        {
            _light.enabled = false;

            StartCoroutine(fadeIn
                ? LerpColor(targetColor, _unActiveMaterial.color, _unActiveMaterial)
                : LerpColor(_unActiveMaterial.color, targetColor));
        }
        
        private IEnumerator LerpColor(Color startColor, Color targetColor, Material endMaterial = null)
        {
            Material blackMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            
            float duration = SequenceController.Instance.TimeBetweenEvents;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                blackMat.color = Color.Lerp(startColor, targetColor, elapsed / duration);
                SetMeshMaterial(blackMat);
                yield return null;
            }

            if (endMaterial == null) endMaterial = blackMat;
            SetMeshMaterial(endMaterial);
        }
        
        private void SetMeshMaterial(Material material)
        {
            Material[] materials = _meshRenderer.materials;
            materials[_buttonMaterialIndex] = material;
            _meshRenderer.materials = materials;
        }
        
        private int GetButtonMaterialIndex()
        {
            Material[] materials = _meshRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].name.Contains("Button"))
                {
                    return i;
                }
            }
            Debug.LogError("No button material found");
            return -1;
        }

        private static Material GetActiveColor(Material materialToCopy)
        {
            Color unActiveColor = materialToCopy.color;
            Color.RGBToHSV(unActiveColor, out float H, out float S, out float V);
            V = 1f;
            Color newColor = Color.HSVToRGB(H, S, V);

            Material material = new Material(materialToCopy)
            {
                name = "ActiveColor",
                color = newColor
            };
            material.SetColor("_EmissionColor", newColor);
            material.EnableKeyword("_EMISSION");
            
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            return material;
        }
    }
}