using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class PixelatedFeatures : ScriptableRendererFeature
{
    PixelatedPass customPass;
    public Material customMaterial;
    public override void Create()
    {
        customPass = new PixelatedPass(customMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var pixelatedSettings = VolumeManager.instance.stack.GetComponent<PixelatedBehaviour>();

        if (pixelatedSettings.intensity != null && customPass.Setup(pixelatedSettings))
        {
            renderer.EnqueuePass(customPass);
        }
    }
}
