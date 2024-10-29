using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelatedPass : ScriptableRenderPass
{
    private Material pixelatedMaterial;
    private PixelatedBehaviour settings;

    private readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    private RenderTargetIdentifier source;

    public PixelatedPass(Material customMaterial)
    {
        pixelatedMaterial = customMaterial;
        renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public bool Setup(PixelatedBehaviour settings)
    {
        this.settings = VolumeManager.instance.stack.GetComponent<PixelatedBehaviour>();
        if (this.settings != null && this.settings.IsActive())
        {
            return true;
        }
        return false;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (pixelatedMaterial == null || settings == null || !settings.IsActive())
            return;

        CommandBuffer cmd = CommandBufferPool.Get("PixelatedPass");

        source = renderingData.cameraData.renderer.cameraColorTargetHandle;

        int width, height;

        // Check if pixelation is active
        if (settings != null && settings.IsActive())
        {
            width = settings.PixelResolution.value;
            height = width * Screen.height / Screen.width;
        }
        else
        {
            // Set to the full screen resolution when the effect is not active
            width = renderingData.cameraData.camera.scaledPixelWidth;
            height = renderingData.cameraData.camera.scaledPixelHeight;
        }
        
        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(width, height, RenderTextureFormat.Default);
        cmd.GetTemporaryRT(temporaryRTIdA, descriptor);

        cmd.Blit(source, temporaryRTIdA, pixelatedMaterial);
        cmd.Blit(temporaryRTIdA, source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd != null)
        {
            cmd.ReleaseTemporaryRT(temporaryRTIdA);
        }
    }
}
