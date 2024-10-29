using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelatedBehaviour : VolumeComponent, IPostProcessComponent
{
    public IntParameter PixelResolution = new IntParameter(0);
    public  bool IsActive() => PixelResolution.value > 0.1f;
    public bool IsTileCompatible() => true;
}
