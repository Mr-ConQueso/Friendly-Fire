using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PosterizationBehaviours : VolumeComponent, IPostProcessComponent
{
    public IntParameter Intensity = new IntParameter(4);


    public bool IsActive() => Intensity.value > 0.1f;

    public bool IsTileCompatible() => true;
}
