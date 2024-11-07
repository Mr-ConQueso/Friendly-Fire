using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DitheringPass : ScriptableRenderPass
{
    public Material ditherMaterial;
    private DitheringBehaviours settings1;
    private PixelatedBehaviour pixelPass;
    private RenderTargetIdentifier source;

    private readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");

    public DitheringPass(Material ditMaterial)
    {
        ditherMaterial = ditMaterial;
        renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public bool Setup(DitheringBehaviours settings)
    {
        settings1 = VolumeManager.instance.stack.GetComponent<DitheringBehaviours>();
        
        pixelPass = VolumeManager.instance.stack.GetComponent<PixelatedBehaviour>();
        if (settings1 != null && settings1.IsActive())
        {
            return true;
        }
        return false;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (ditherMaterial == null || settings1 == null || !settings1.IsActive())
        {
            Debug.Log(settings1);
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get("DitheringPass");
        
        // Utiliza renderingData para obtener el color target
        if(pixelPass == null|| pixelPass.IsActive())
        {
            
        }
        else
        {
            ditherMaterial.SetInt("_ScreenHeight", renderingData.cameraData.camera.scaledPixelHeight);
            ditherMaterial.SetInt("_ScreenWidth", renderingData.cameraData.camera.scaledPixelWidth);
        }
        source = renderingData.cameraData.renderer.cameraColorTargetHandle;           

        RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        cmd.GetTemporaryRT(temporaryRTIdA, opaqueDesc);

        cmd.Blit( source, temporaryRTIdA, ditherMaterial);
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
