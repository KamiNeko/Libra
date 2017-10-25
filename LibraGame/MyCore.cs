using LibraCore.Scenes;
using Nez;

namespace LibraCore
{
    public class MyCore : Core
    {
        public MyCore() : base(width: 1280, height: 960, isFullScreen: false, enableEntitySystems: false)
        {
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            
            scene = new LevelScene();
        }
    }
}
