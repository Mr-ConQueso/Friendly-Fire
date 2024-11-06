using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DitheringFeatures : ScriptableRendererFeature
{
    DitheringPass DitPass;
    [SerializeField] Material postMaterial;
    public override void Create()
    {
        DitPass = new DitheringPass(postMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var ditheringSettings = VolumeManager.instance.stack.GetComponent<DitheringBehaviours>();

        if (ditheringSettings.Intensity != null && DitPass.Setup(ditheringSettings))
        {
            renderer.EnqueuePass(DitPass);
        }
    }
}
