using UnityEngine;
using UnityEngine.Rendering;

public class OpaqueRenderPipeline : RenderPipeline
{
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            context.SetupCameraProperties(camera);
            context.DrawSkybox(camera);
            var cullParam = new ScriptableCullingParameters();
            camera.TryGetCullingParameters(out cullParam);
            cullParam.isOrthographic = false;
            
            var cullResults = context.Cull(ref cullParam);
            var sortSet = new SortingSettings(camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };
            var drawSet = new DrawingSettings(new ShaderTagId("Always"), sortSet);
            var filtSet = new FilteringSettings(RenderQueueRange.opaque, -1);
            context.DrawRenderers(cullResults, ref drawSet, ref filtSet);
            
            context.Submit();
        }
    }
}
