using Microsoft.Xna.Framework;
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

            //var instructionsEntity = CreateEntity("instructions");
            //instructionsEntity.addComponent(new TextSprite(Graphics.instance.bitmapFont, "HELLO WORLD", new Vector2(10, 10), Color.White)).SetRenderLayer(ScreenSpaceRenderLayer);

            //table = uiCanvas.stage.addElement(new Table());
            //table.setFillParent(true).right().top();

            //var topButtonStyle = new TextButtonStyle(new PrimitiveDrawable(Color.Black, 10f), new PrimitiveDrawable(Color.Yellow), new PrimitiveDrawable(Color.DarkSlateBlue))
            //{
            //    downFontColor = Color.White
            //};
            //table.add(new TextButton("Toggle Scene List", topButtonStyle)).setFillX().setMinHeight(30).getElement<Button>().onClicked += onToggleSceneListClicked;

            //table.row().setPadTop(10);
            //var checkbox = table.add(new CheckBox("Debug Render", new CheckBoxStyle
            //{
            //    checkboxOn = new PrimitiveDrawable(30, Color.Green),
            //    checkboxOff = new PrimitiveDrawable(30, new Color(0x00, 0x3c, 0xe7, 0xff))
            //})).getElement<CheckBox>();
            //checkbox.onChanged += enabled => Core.debugRenderEnabled = enabled;
            //checkbox.isChecked = Core.debugRenderEnabled;
            //table.row().setPadTop(30);

            //var buttonStyle = new TextButtonStyle(new PrimitiveDrawable(new Color(78, 91, 98), 10f), new PrimitiveDrawable(new Color(244, 23, 135)), new PrimitiveDrawable(new Color(168, 207, 115)))
            //{
            //    downFontColor = Color.Black
            //};
        }

        public override void Update()
        {
            base.Update();
        }

        protected const int ScreenSpaceRenderLayer = 999;
        protected UICanvas uiCanvas;
        protected Table table;
    }
}
