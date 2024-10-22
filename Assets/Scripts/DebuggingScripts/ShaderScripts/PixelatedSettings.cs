using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Pixel")]
public class PixelatedSettings :VolumeComponent, IPostProcessComponent
{
    public bool IsActive()
    {
        return false;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}

