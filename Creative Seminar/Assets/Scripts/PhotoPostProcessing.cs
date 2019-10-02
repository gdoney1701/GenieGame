﻿using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(GrayscaleRenderer), PostProcessEvent.AfterStack, "Custom/Grayscale")]
public sealed class PhotoPostProcessing : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
}

public sealed class GrayscaleRenderer : PostProcessEffectRenderer<PhotoPostProcessing>
{
    public override void Render(PostProcessRenderContext context)
    {

        var sheet = context.propertySheets.Get(Shader.Find("Custom/Grayscale"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0, false);
    }
}
