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

         }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(_pixelTextID);
        base.FrameCleanup(cmd);
    }
}
