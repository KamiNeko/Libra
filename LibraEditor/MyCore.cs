using LibraCore.Scenes;
using Nez;
using System;

namespace LibraEditor
{
    public class MyCore : Core
    {
        public MyCore() : base(width: 1280, height: 960, isFullScreen: false, enableEntitySystems: false)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            ShowTitleScreen();
        }

        private void ShowTitleScreen()
        {
            var titleScreen = new TitleScreen();
            titleScreen.TitleScreenSkipped += OnTitleScreenSkipped;

            StartTransitionToNextScene(titleScreen);
        }

        private void OnTitleScreenSkipped(object sender, EventArgs e)
        {
            var titleScreen = sender as TitleScreen;
            titleScreen.TitleScreenSkipped -= OnTitleScreenSkipped;

            var levelScene = new LevelScene() { LevelEditorModeActive = false };
            levelScene.GameWon += OnGameWon;
            levelScene.GameOver += OnGameOver;

            StartTransitionToNextScene(levelScene);
        }

        private void OnGameWon(object sender, EventArgs e)
        {
            var levelScene = sender as LevelScene;
            UnregisterLevelSceneHandlers(levelScene);
            ShowTitleScreen();
        }

        private void OnGameOver(object sender, EventArgs e)
        {
            var levelScene = sender as LevelScene;
            UnregisterLevelSceneHandlers(levelScene);
            ShowTitleScreen();
        }

        private void UnregisterLevelSceneHandlers(LevelScene levelScene)
        {
            levelScene.GameWon -= OnGameWon;
            levelScene.GameOver -= OnGameOver;
        }

        private void StartTransitionToNextScene(Scene nextScene)
        {
            startSceneTransition(GetSceneTransition(() => nextScene));
            scene = nextScene;
        }

        private SceneTransition GetSceneTransition(Func<Scene> sceneLoader)
        {
            return new FadeTransition() { fadeInDuration = 0.5f, fadeOutDuration = 0.5f, delayBeforeFadeInDuration = 0.0f };
        }
    }
}
