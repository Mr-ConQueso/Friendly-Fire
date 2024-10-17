using UnityEngine.Rendering.Universal;

public class BlurRendererFeature : ScriptableRendererFeature
{
    private BlurRenderPass _blurRenderPass;

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        _blurRenderPass.Setup(renderer.cameraColorTargetHandle);  // use of target after allocation
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
