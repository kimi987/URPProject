using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ColorRenderPipeline : RenderPipeline
{
    private CommandBuffer _cb = new CommandBuffer();

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _cb?.Dispose();
        _cb = null;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            context.SetupCameraProperties(camera);
            _cb.ClearRenderTarget(true, true, Color.blue);
            context.ExecuteCommandBuffer(_cb);
            _cb.Clear();
            context.Submit();
        }
    }
}
