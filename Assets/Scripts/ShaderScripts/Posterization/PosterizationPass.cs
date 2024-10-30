using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PosterizationPass : ScriptableRenderPass
{
    public Material posterMaterial;
    private PosterizationBehaviours settings1;
    private RenderTargetIdentifier source;

    private readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");

    public PosterizationPass(Material posMaterial)
    {
        posterMaterial = posMaterial;
        renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public bool Setup(PosterizationBehaviours settings)
    {
        settings1 = VolumeManager.instance.stack.GetComponent<PosterizationBehaviours>();
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
        cmd.Blit(source,temporaryRTIdA);

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
