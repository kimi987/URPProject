using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/ColorRenderPipeline")]
public class ColorRenderAssets : RenderPipelineAsset
{
    protected override RenderPipeline CreatePipeline()
    {
        return new ColorRenderPipeline();
    }
}
