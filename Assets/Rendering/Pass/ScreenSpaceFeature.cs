using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Experimental.Rendering.Universal
{
    public class ScreenSpaceFeature : ScriptableRendererFeature
    {

        [System.Serializable]
        public class BasicFeatureSettings
        {

            public LayerMask layerMask = 0;

            public RenderPassEvent Event = RenderPassEvent.BeforeRenderingTransparents;

            public Material blitMat = null;
        }

        public BasicFeatureSettings settings = new BasicFeatureSettings();

        ScreenSpacePass pass;

        public override void Create()
        {
            pass = new ScreenSpacePass(settings.Event, settings.blitMat, settings.layerMask);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(pass);
        }

    }
}