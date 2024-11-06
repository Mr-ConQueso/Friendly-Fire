using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DitheringBehaviours : VolumeComponent, IPostProcessComponent
{
    public IntParameter Intensity = new IntParameter(0);


    public bool IsActive() => Intensity.value > 0.1f;

    public bool IsTileCompatible() => true;
}
