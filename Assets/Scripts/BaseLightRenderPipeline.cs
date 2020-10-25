using UnityEngine;
using UnityEngine.Rendering;

public class BaseLightRenderPipeline : RenderPipeline
{
    private CommandBuffer _commandBuffer = new CommandBuffer();
   protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
        {
            //渲染开始后，创建CommandBuffer;
            if (_commandBuffer == null) _commandBuffer = new CommandBuffer() { name = "SRP Study CB"};

            //将shader中需要的属性参数映射为ID，加速传参
            var _LightDir0 = Shader.PropertyToID("_DLightDir");
            var _LightColor0 = Shader.PropertyToID("_DLightColor");   
            //同上一节，所有相机开始逐次渲染
            foreach (var camera in cameras)
            {                
                //设置渲染相关相机参数,包含相机的各个矩阵和剪裁平面等
                renderContext.SetupCameraProperties(camera);               
                //清理myCommandBuffer，设置渲染目标的颜色为灰色。
                _commandBuffer.ClearRenderTarget(true, true, Color.gray);                

                //同上一节的剪裁
                ScriptableCullingParameters cullParam = new ScriptableCullingParameters();
                camera.TryGetCullingParameters(out cullParam);     
                cullParam.isOrthographic = false;
                CullingResults cullResults =  renderContext.Cull(ref cullParam);
                
                //在剪裁结果中获取灯光并进行参数获取
                var lights = cullResults.visibleLights;
                _commandBuffer.name = "Render Lights";   
                foreach (var light in lights)
                {
                    //判断灯光类型
                    if (light.lightType != LightType.Directional) continue;           
                    //获取灯光参数,平行光朝向即为灯光Z轴方向。矩阵第一到三列分别为xyz轴项，第四列为位置。
                    Vector4 lightpos = light.localToWorldMatrix.GetColumn(2);
                    //灯光方向反向。默认管线中，unity提供的平行光方向也是灯光反向。光照计算决定
                    Vector4 lightDir = -lightpos;
                    //方向的第四个值(W值)为0，点为1.
                    lightDir.w = 0;
                    //这边获取的灯光的finalColor是灯光颜色乘上强度之后的值，也正好是shader需要的值
                    Color lightColor = light.finalColor;
                    Debug.LogError("lightColor = " + lightColor.ToString());
                    //利用CommandBuffer进行参数传递。
                    _commandBuffer.SetGlobalVector(_LightDir0, lightDir);
                    _commandBuffer.SetGlobalColor(_LightColor0, Color.blue);
                    break;                  
                }
                //执行CommandBuffer中的指令
                renderContext.ExecuteCommandBuffer(_commandBuffer);
                _commandBuffer.Clear();

                //同上节，过滤
                FilteringSettings filtSet = new FilteringSettings(RenderQueueRange.opaque, -1);
                //filtSet.renderQueueRange = RenderQueueRange.opaque;
                //filtSet.layerMask = -1;                

                //同上节，设置Renderer Settings
                //注意在构造的时候就需要传入Lightmode参数，对应shader的pass的tag中的LightMode
                SortingSettings sortSet = new SortingSettings(camera) { criteria = SortingCriteria.CommonOpaque };
                DrawingSettings drawSet = new DrawingSettings(new ShaderTagId("BaseLit"), sortSet);

                //绘制物体
                renderContext.DrawRenderers(cullResults, ref drawSet, ref filtSet);
                
                //绘制天空球
                renderContext.DrawSkybox(camera);
                //开始执行渲染内容
                renderContext.Submit();
            }
        }
}
