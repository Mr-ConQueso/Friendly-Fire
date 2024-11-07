using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WorldSpaceOverlayUI : MonoBehaviour
{
    private static readonly int UnityGuizTestMode = Shader.PropertyToID(ShaderTestMode);
    private const string ShaderTestMode = "unity_GUIZTestMode"; //The magic property we need to set
    [SerializeField] private UnityEngine.Rendering.CompareFunction _desiredUIComparison = UnityEngine.Rendering.CompareFunction.Always; //If you want to try out other effects
    [Tooltip("Set to blank to automatically populate from the child UI elements")]
    [SerializeField] private Graphic[] _uiElementsToApplyTo;

    //Allows us to reuse materials
    private readonly Dictionary<Material, Material> _materialMappings = new Dictionary<Material, Material>();

    protected virtual void Start()
    {
        if (_uiElementsToApplyTo.Length == 0)
        {
            _uiElementsToApplyTo = gameObject.GetComponentsInChildren<Graphic>();
        }

        foreach (var graphic in _uiElementsToApplyTo)
        {
            Material material = graphic.material;
            if (material == null)
            {
                Debug.LogError($"{nameof(WorldSpaceOverlayUI)}: skipping target without material {graphic.name}.{graphic.GetType().Name}");
                continue;
            }

            if (!_materialMappings.TryGetValue(material, out Material materialCopy))
            {
                materialCopy = new Material(material);
                _materialMappings.Add(material, materialCopy);
            }

            material.SetInt(UnityGuizTestMode, (int)_desiredUIComparison);
            //graphic.material = materialCopy;
        }
    }
}