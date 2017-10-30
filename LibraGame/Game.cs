using LibraCore.Scenes;
using Nez;
using System;

namespace LibraGame
{
    public class Game : Core
    {
        public Game() : base(width: 1280, height: 960, isFullScreen: false, enableEntitySystems: true)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            Transform.shouldRoundPosition = false;
            ShowTitleScreen();
        }

        private void ShowTitleScreen()
        {
            if (!transitionIsActive)
            {
                var titleScreen = new TitleScreen();
                titleScreen.TitleScreenSkipped += OnTitleScreenSkipped;

                if (scene == null)
                {
                    scene = titleScreen;
                }

                StartTransitionToNextScene(titleScreen);
            }
        }

        private void OnTitleScreenSkipped(object sender, EventArgs e)
        {
            if (!transitionIsActive)
            {
                var titleScreen = sender as TitleScreen;
                titleScreen.TitleScreenSkipped -= OnTitleScreenSkipped;

                var levelScene = new LevelScene() { LevelEditorModeActive = true };
                levelScene.GameWon += OnGameWon;
                levelScene.GameOver += OnGameOver;

                StartTransitionToNextScene(levelScene);
            }
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
            var transition = GetSceneTransition(() => nextScene);
            transition.onTransitionCompleted = () => { transitionIsActive = false; };
            startSceneTransition(transition);
            scene = nextScene;
            transitionIsActive = true;
        }

        private SceneTransition GetSceneTransition(Func<Scene> sceneLoader)
        {
            return new FadeTransition() { fadeInDuration = 0.5f, fadeOutDuration = 0.5f, delayBeforeFadeInDuration = 0.0f };
        }

        private bool transitionIsActive;
    }
}
