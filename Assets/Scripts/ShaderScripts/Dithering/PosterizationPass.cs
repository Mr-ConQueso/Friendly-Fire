using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DitheringPass : ScriptableRenderPass
{
    public Material posterMaterial;
    private DitheringBehaviours settings1;
    private RenderTargetIdentifier source;

    private readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");

    public DitheringPass(Material posMaterial)
    {
        posterMaterial = posMaterial;
        renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public bool Setup(DitheringBehaviours settings)
    {
        settings1 = VolumeManager.instance.stack.GetComponent<DitheringBehaviours>();
        if (settings1 != null && settings1.IsActive())
        {
            return true;
        }
        return false;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (posterMaterial == null || settings1 == null || !settings1.IsActive())
        {
            Debug.Log(settings1);
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get("PosterizationPass");
        
        // Utiliza renderingData para obtener el color target
        source = renderingData.cameraData.renderer.cameraColorTargetHandle;           
        posterMaterial.SetFloat("_Levels", settings1.Intensity.value);

        RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        cmd.GetTemporaryRT(temporaryRTIdA, opaqueDesc);

        cmd.Blit( source, temporaryRTIdA, posterMaterial);
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
