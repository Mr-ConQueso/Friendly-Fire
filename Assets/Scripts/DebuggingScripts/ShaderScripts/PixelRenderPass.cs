using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class PixelRenderPass : ScriptableRenderPass
{
    private Material _material;
    private PixelatedSettings _pixelSettings;
    private RenderTargetIdentifier _source;
    private int _pixelTextID;
   private static readonly int PixelSize = Shader.PropertyToID("_PixelSize");

    public bool SetUp(RenderTargetIdentifier renderTarget, Shader gaussianPixelShader)
    {
        _source = renderTarget;
        _pixelSettings = VolumeManager.instance.stack.GetComponent<PixelatedSettings>();
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        if(_pixelSettings == null && _pixelSettings.IsActive())
        {
            _material = new Material(gaussianPixelShader);
            return true;
        }
        return false;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        if (_pixelSettings == null || !_pixelSettings.IsActive())
        {
            return;
        }
        _pixelTextID = Shader.PropertyToID("Pixel");
        cmd.GetTemporaryRT(_pixelTextID, cameraTextureDescriptor);
        base.Configure(cmd, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.SceneView && !renderingData.cameraData.postProcessEnabled)
        {
            return;
        }

        if (_pixelSettings == null || !_pixelSettings.IsActive())
        {
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get("Pixelate Post Process");

        // Set pixel size property based on the strength from _pixelSettings
        int pixelSize = Mathf.Max(1, Mathf.CeilToInt(_pixelSettings.pixelSize.value));
        _material.SetInteger(PixelSize, pixelSize);

        // Execute the pixelation effect
        cmd.Blit(_source, _pixelTextID, _material, 0);  // First pass: apply pixelation to temporary texture
        cmd.Blit(_pixelTextID, _source);                // Second pass: render back to the source

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(_pixelTextID);
        base.FrameCleanup(cmd);
    }
}
