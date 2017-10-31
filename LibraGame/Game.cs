using LibraCore.Scenes;
using Nez;
using System;

namespace LibraGame
{
    public class Game : Core
    {
        public Game() : base(width: 640, height: 480, isFullScreen: false, enableEntitySystems: true, windowTitle: "Libra")
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
                titleScreen.SceneSkipped += OnTitleScreenSkipped;

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
                titleScreen.SceneSkipped -= OnTitleScreenSkipped;

                var levelScene = new LevelScene();
                levelScene.GameWon += OnGameWon;
                levelScene.GameOver += OnGameOver;

                StartTransitionToNextScene(levelScene);
            }
        }

        private void OnGameWon(object sender, EventArgs e)
        {
            var levelScene = sender as LevelScene;
            UnregisterLevelSceneHandlers(levelScene);

            var gameWonScene = new GameWonScene();
            gameWonScene.SceneSkipped += OnGameWonSceneSkipped;
            StartTransitionToNextScene(gameWonScene);
        }

        private void OnGameWonSceneSkipped(object sender, EventArgs e)
        {
            if (!transitionIsActive)
            {
                var gameWonScene = sender as GameWonScene;
                gameWonScene.SceneSkipped -= OnGameWonSceneSkipped;

                ShowTitleScreen();
            }
        }

        private void OnGameOver(object sender, EventArgs e)
        {
            var levelScene = sender as LevelScene;
            UnregisterLevelSceneHandlers(levelScene);

            var gameOverScene = new GameOverScene();
            gameOverScene.SceneSkipped += OnGameOverSceneSkipped;
            StartTransitionToNextScene(gameOverScene);
        }

        private void OnGameOverSceneSkipped(object sender, EventArgs e)
        {
            if (!transitionIsActive)
            {
                var gameOverScene = sender as GameOverScene;
                gameOverScene.SceneSkipped -= OnGameOverSceneSkipped;

                ShowTitleScreen();
            }
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
