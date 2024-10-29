using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelatedBehaviour : VolumeComponent, IPostProcessComponent
{
    public IntParameter intensity = new IntParameter(516);
    public  bool IsActive() => intensity.value > 0.1f;
    public bool IsTileCompatible() => true;
}
