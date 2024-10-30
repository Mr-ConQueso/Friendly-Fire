using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PosterizationFeatures : ScriptableRendererFeature
{
    PosterizationPass postPass;
    [SerializeField] Material postMaterial;
    public override void Create()
    {
        postPass = new PosterizationPass(postMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var posterizationSettings = VolumeManager.instance.stack.GetComponent<PosterizationBehaviours>();

        if (posterizationSettings.Intensity != null && postPass.Setup(posterizationSettings))
        {
            renderer.EnqueuePass(postPass);
        }
    }
}
