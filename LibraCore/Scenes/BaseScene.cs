using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;

namespace LibraCore.Scenes
{
    public class BaseScene : Scene
    {
        public BaseScene()
        {
            AddRenderer(new ScreenSpaceRenderer(100, ScreenSpaceRenderLayer));
            AddRenderer(new RenderLayerExcludeRenderer(0, ScreenSpaceRenderLayer));

            InitializeUI();
        }

        public override void Initialize()
        {
            base.Initialize();

            ClearColor = Color.Black;
            SetDesignResolution(640, 480, SceneResolutionPolicy.ShowAllPixelPerfect);
        }

        private void InitializeUI()
        {
            uiCanvas = CreateEntity("uiCanvas").addComponent(new UICanvas());
            uiCanvas.isFullScreen = false;
            uiCanvas.RenderLayer = ScreenSpaceRenderLayer;
        }

        public override void Update()
        {
            base.Update();

            if (Input.isKeyReleased(Keys.F))
            {
                Screen.isFullscreen = !Screen.isFullscreen;
                Screen.applyChanges();
            }
        }

        protected const int ScreenSpaceRenderLayer = 999;
        protected UICanvas uiCanvas;
        protected Table table;
    }
}
