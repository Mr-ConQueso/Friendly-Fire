using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Blur")]
public class BlurSettings : VolumeComponent, IPostProcessComponent
{
    [Tooltip("Standard deviation (spread) of the blur. Grid size is approx. 3x larger.")]
    public ClampedFloatParameter Strength = new ClampedFloatParameter(0.0f, 0.0f, 15.0f);
    
    [Tooltip("Select a blurring algorithm to use for the blurring process.")]
    public BlurModeParameter BlurMode = new BlurModeParameter(global::BlurMode.GaussianBlur);

    public bool IsActive()
    {
        return (Strength.value > 0.0f) && active;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}

[Serializable]
public class BlurModeParameter : VolumeParameter<BlurMode>
{
    public BlurModeParameter(BlurMode value, bool overrideState = false) : base(value, overrideState) { }
}

public enum BlurMode
{
    GaussianBlur,
    BoxBlur
}
