using System;
using UnityEngine;
using UnityEngine.Rendering;

public class SkyBoxRenderPipeline : RenderPipeline
{
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
           context.SetupCameraProperties(camera);
           context.DrawSkybox(camera);
           context.Submit();
        }
    }
}
