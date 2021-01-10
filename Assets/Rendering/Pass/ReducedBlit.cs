using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class ReducedBlit : ScriptableRendererFeature
{
    [System.Serializable]
    class CustomRenderPass : ScriptableRenderPass
    {
        public RenderTargetIdentifier source;
        private RenderTargetHandle tempRenderTargetHandle;
        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.

        private Material material;
        private float resizeAmount;

        public CustomRenderPass(Material material, RenderPassEvent passEvent, float resizeAmount)
        {
            this.material = material;
            tempRenderTargetHandle.Init("_ReducedBlitTexture");
            this.resizeAmount = resizeAmount;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            ref var cam = ref renderingData.cameraData.camera;
            cmd.GetTemporaryRT(tempRenderTargetHandle.id, cam.pixelWidth / (int)resizeAmount, cam.pixelHeight / (int)resizeAmount, 0, FilterMode.Point);
            Blit(cmd, source, tempRenderTargetHandle.Identifier(), material, 0);
            Blit(cmd, tempRenderTargetHandle.Identifier(), source, passIndex: 1);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {
        }


    }

    [System.Serializable]
    public class Settings
    {
        public Material material = null;
        public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;
        public float resizeAmount = 2;
    }


    public Settings settings = new Settings();

    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(settings.material, settings.Event, settings.resizeAmount);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = settings.Event;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        m_ScriptablePass.source = renderer.cameraColorTarget;
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


