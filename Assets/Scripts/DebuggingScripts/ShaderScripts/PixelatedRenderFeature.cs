using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pixelated : ScriptableRendererFeature
{

    [Tooltip("The Pixelated Shader")]
    [SerializeField] private Shader pixelatedShader;

    public override void Create()
    {
        
    }

 public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
       
    }
}
