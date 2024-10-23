using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlurRendererFeature : ScriptableRendererFeature
{
    [Tooltip("The blur shader.")]
    [SerializeField] private Shader blurShader;
    
    private BlurRenderPass _blurRenderPass;

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        // Updated to use URP 14.0
        _blurRenderPass.Setup(renderer.cameraColorTargetHandle, blurShader);  // use of target after allocation
    }

    public override void Create()
    {
        _blurRenderPass = new BlurRenderPass();
        name = "Blur";
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_blurRenderPass);
    }
}
