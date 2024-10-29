using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        renderer.EnqueuePass(postPass);
    }
}
