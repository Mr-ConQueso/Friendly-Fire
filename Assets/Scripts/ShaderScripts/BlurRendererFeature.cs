using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlurRendererFeature : ScriptableRendererFeature
{
    [Tooltip("The Gaussian blurring shader.")]
    [SerializeField] private Shader gaussianBlurShader;
    
    [Tooltip("The Box blurring shader.")]
    [SerializeField] private Shader boxBlurShader;
    
    private BlurRenderPass _blurRenderPass;

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        _blurRenderPass.Setup(renderer.cameraColorTargetHandle, gaussianBlurShader, boxBlurShader);  // use of target after allocation
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
