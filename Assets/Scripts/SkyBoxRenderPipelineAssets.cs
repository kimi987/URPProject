using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/SkyBoxRenderPipeline")]
public class SkyBoxRenderPipelineAssets : RenderPipelineAsset
{
    protected override RenderPipeline CreatePipeline()
    {
        return new SkyBoxRenderPipeline();
    }
}
