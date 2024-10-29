using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PosterizationPass : ScriptableRenderPass
{
    public Material posterMaterial;
    public PosterizationBehaviours settings;
    private RenderTargetIdentifier source;

    private readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    public bool Setup()
    {
        settings = VolumeManager.instance.stack.GetComponent<PosterizationBehaviours>();
        if(settings != null || settings.IsActive())
        {
            return true;
        }
        return false;
    }
    public PosterizationPass(Material posMaterial)
    {
        posMaterial = posterMaterial;
        renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (posterMaterial == null || settings == null || !settings.IsActive())
            return;

        CommandBuffer cmd = CommandBufferPool.Get("PosterizationPass");
        source = renderingData.cameraData.renderer.cameraColorTargetHandle;
        posterMaterial.SetFloat("_Levels", settings.Intensity.value);

        RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        cmd.GetTemporaryRT(temporaryRTIdA, opaqueDesc);

        cmd.Blit( source, temporaryRTIdA, posterMaterial);
        cmd.Blit( temporaryRTIdA, source);

        cmd.ReleaseTemporaryRT(temporaryRTIdA);

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
