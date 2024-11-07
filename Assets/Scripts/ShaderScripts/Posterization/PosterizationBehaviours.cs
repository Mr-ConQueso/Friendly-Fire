using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PosterizationBehaviours : VolumeComponent, IPostProcessComponent
{
    public ColorParameter color = new ColorParameter(Color.black);
    public FloatParameter saturation = new FloatParameter(0);
    public IntParameter Value = new IntParameter(0);

    public bool IsActive() => Value.value > 0.1f;

    public bool IsTileCompatible() => true;
}
