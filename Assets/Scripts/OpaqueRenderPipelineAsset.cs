using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/OpaqueRenderPipelineAsset")]
public class OpaqueRenderPipelineAsset : RenderPipelineAsset
{
    protected override RenderPipeline CreatePipeline()
    {
        return new OpaqueRenderPipeline();
    }
}
