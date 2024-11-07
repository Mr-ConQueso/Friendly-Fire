using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InverseImageMask : Image
{
    private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");
    private Material _maskMaterial;
    
    public override Material materialForRendering
    {
        get
        {
            if (_maskMaterial != null) return _maskMaterial;
            
            _maskMaterial = new Material(base.materialForRendering);
            _maskMaterial.SetInt(StencilComp, (int)CompareFunction.NotEqual);
            return _maskMaterial;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _maskMaterial = null;
    }
}