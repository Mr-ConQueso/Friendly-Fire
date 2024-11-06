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
        private Light _light;
        private Material _unActiveMaterial, _activeMaterial;
        private int _buttonMaterialIndex;
        
        public void DisableInteractable()
        {
            _isInteractable = false;
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

        private void OnMouseDown()
        {
            if (_isInteractable) 
            {
                if (_isActive)
                {
                    DeactivateButton();
                    SequenceController.Instance.ClearSequence();
                }
                else
                {
                    ActivateButton();
                    SequenceController.Instance.ClickSequencedObject(ObjectIndex);
                    Invoke(nameof(DeactivateButton), 0.5f);
                }
            }
        }

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
        
        private void SetActiveColor(Color color)
        {
            Material blackMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            blackMat.name = "Black";
            blackMat.color = color;
            SetMeshMaterial(blackMat);
            _light.enabled = false;
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

        private Material GetActiveColor(Material materialToCopy)
        {
            Color unactiveColor = materialToCopy.color;
            Color.RGBToHSV(unactiveColor, out float H, out float S, out float V);
            V = 1f;
            Color newColor = Color.HSVToRGB(H, S, V);

            Material material = new Material(materialToCopy);
            material.name = "ActiveColor";
            material.color = newColor;
            material.SetColor("_EmissionColor", newColor);
            material.EnableKeyword("_EMISSION");
            
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            return material;
        }
    }
}