using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRenderPass : ScriptableRenderPass
{
    private Material _material;
    private BlurSettings _blurSettings;

    private RenderTargetIdentifier _source;
    private int _blurTexID;
    private static readonly int GridSize = Shader.PropertyToID("_GridSize");
    private static readonly int Spread = Shader.PropertyToID("_Spread");

    public bool Setup(RenderTargetIdentifier renderTarget)
    {
        _source = renderTarget;
        _blurSettings = VolumeManager.instance.stack.GetComponent<BlurSettings>();
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        if (_blurSettings != null && _blurSettings.IsActive())
        {
            _material = new Material(Shader.Find("PostProcessing/Blur"));
            return true;
        }

        return false;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        if (_blurSettings == null || !_blurSettings.IsActive())
        {
            return;
        }

        _blurTexID = Shader.PropertyToID("_BlurTex");
        cmd.GetTemporaryRT(_blurTexID, cameraTextureDescriptor);

        base.Configure(cmd, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData is { cameraType: CameraType.SceneView, postProcessEnabled: false })
        {
            return;
        }
        
        if (_blurSettings == null || !_blurSettings.IsActive())
        {
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get("Blur Post Process");

        // Set Blur effect properties.
        int gridSize = Mathf.CeilToInt(_blurSettings.strength.value * 3.0f);

        if(gridSize % 2 == 0)
        {
            gridSize++;
        }

        _material.SetInteger(GridSize, gridSize);
        _material.SetFloat(Spread, _blurSettings.strength.value);

        // Execute effect using effect material with two passes.
        cmd.Blit(_source, _blurTexID, _material, 0);
        cmd.Blit(_blurTexID, _source, _material, 1);

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(_blurTexID);
        base.FrameCleanup(cmd);
    }
}