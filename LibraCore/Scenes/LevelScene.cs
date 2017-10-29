using Microsoft.Xna.Framework;
using LibraCore.Components;
using LibraCore.LevelBuilding;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using LibraCore.Systems;

namespace LibraCore.Scenes
{
    public class LevelScene : BaseScene
    {
        public EventHandler GameOver;
        public EventHandler GameWon;

        public bool LevelEditorModeActive { get; set; }

        public override void Initialize()
        {
            base.Initialize();

            AddEntityProcessor(new PlayerControllerSystem());
            AddEntityProcessor(new BulletSystem());
            AddEntityProcessor(new BulletControllerSystem());

            CreateLevelDescriptors();
            SwitchToNextLevel();
            CreateRemainingLifesText();
            explosionSoundEffect = ContentManager.Load<SoundEffect>("explosion");
        }

        public override void Update()
        {
            base.Update();

            if (!sceneTransitionIsActive)
            {
                var spaceshipEntity = Entities.findEntity(LevelConstants.SpaceshipEntiyName);

                if (ShipLeftLevel())
                {
                    if (LevelEditorModeActive)
                    {
                        spaceshipEntity.getComponent<Sprite>().Color = Color.Green;
                    }
                    else
                    {
                        SwitchToNextLevel();
                    }
                }
                else if (ShipHasCollisions())
                {
                    if (LevelEditorModeActive)
                    {
                        spaceshipEntity.getComponent<Sprite>().Color = Color.Red;
                    }
                    else
                    {
                        HandleShipCollision();
                    }
                }
                else if (LevelEditorModeActive)
                {
                    spaceshipEntity.getComponent<Sprite>().Color = Color.White;
                }
            }
        }
        
        private void HandleShipCollision()
        {
            lifes--;

            explosionSoundEffect.Play();

            if (lifes <= 0)
            {
                HandleGameOver();
            }
            else
            {
                var transition = new FadeTransition() { fadeInDuration = 0.2f, fadeOutDuration = 0.2f, delayBeforeFadeInDuration = 0.0f };

                transition.onScreenObscured = () =>
                {
                    ResetLevel();
                };

                transition.onTransitionCompleted = () =>
                {
                    sceneTransitionIsActive = false;
                };

                Core.startSceneTransition(transition);
                sceneTransitionIsActive = true;
            }
        }

        private void ResetLevel()
        {
            EntityProcessors.getProcessor<BulletSystem>().Reset();
            EntityProcessors.getProcessor<BulletControllerSystem>().Reset();

            var currentLevelDescriptor = GetCurrentLevelDescriptor();
            var spaceshipEntity = Entities.findEntity(LevelConstants.SpaceshipEntiyName);
            spaceshipEntity.setPosition(currentLevelDescriptor.StartPosition);

            RecreateRemainingLifesText();
        }

        private bool ShipHasCollisions()
        {
            var entity = Entities.findEntity("space-ship");
            var component = entity.getComponent<CollisionTesterComponent>();

            return component.HasCollisions;
        }

        private bool ShipLeftLevel()
        {
            var entity = Entities.findEntity("space-ship");
            var component = entity.getComponent<EntityBoundsOutOfScreenTesterComponent>();
            return component.EntityBoundsOutOfScreen;
        }

        private void CreateLevelDescriptors()
        {
            foreach (var levelName in LoadLevelNames().OrderBy(x => x))
            {
                levelDescriptors.Add(BuildLevelDescriptor(levelName));
            }
        }

        private LevelDescriptor BuildLevelDescriptor(string levelFileName)
        {
            return new LevelDescriptorReader(levelFileName).Load();
        }

        private string[] LoadLevelNames()
        {
            if (!File.Exists(LevelConstants.LevelNamesFileName))
            {
                var sampleLevelNamesFile = Newtonsoft.Json.JsonConvert.SerializeObject(new[] { "level01.json", "level02.json", "level03.json" }, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(LevelConstants.LevelNamesFileName, sampleLevelNamesFile);

                throw new Exception("Could not find level names file, I am creating a default one as template - please edit it to match the level name(s) before you restart the application");
            }

            var levelNamesFileContent = File.ReadAllText(LevelConstants.LevelNamesFileName);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(levelNamesFileContent);
        }

        private void SwitchToNextLevel()
        {
            UnloadAllEntites();

            currentLevel++;

            if (currentLevel > levelDescriptors.Count)
            {
                HandleGameWon();
            }
            else
            {
                LoadEntitesOfCurrentLevelDescriptor();
            }
        }

        private void LoadEntitesOfCurrentLevelDescriptor()
        {
            var currentLevelDescriptor = GetCurrentLevelDescriptor();
            var levelLoader = new LevelBuilder(ContentManager, currentLevelDescriptor);
            var entites = levelLoader.BuildEntites();

            foreach (var entity in entites)
            {
                AddEntity(entity);
            }
        }

        private void RecreateRemainingLifesText()
        {
            DestroyRemainingLifesText();
            CreateRemainingLifesText();
        }

        private void DestroyRemainingLifesText()
        {
            var textEntity = Entities.findEntity(RemainingLifesTextEntityName);
            textEntity.detachFromScene();
            Entities.remove(textEntity);
        }

        private void CreateRemainingLifesText()
        {
            var textEntity = CreateEntity(RemainingLifesTextEntityName);
            textEntity.addComponent(new TextSprite(Graphics.instance.bitmapFont, $"LIFES: {lifes}", new Vector2(580, 460), Color.Black)).SetRenderLayer(ScreenSpaceRenderLayer);
        }

        private void HandleGameWon()
        {
            GameWon?.Invoke(this, EventArgs.Empty);
        }

        private void HandleGameOver()
        {
            GameOver?.Invoke(this, EventArgs.Empty);
        }

        private void UnloadAllEntites()
        {
            // TODO: Test this, I think it will also throw out stuff we need (such as camera)
            Entities.removeAllEntities();
        }

        private LevelDescriptor GetCurrentLevelDescriptor()
        {
            var currentLevelDescriptorIndex = currentLevel - 1;
            var currentLevelDescriptor = levelDescriptors.ElementAt(currentLevelDescriptorIndex);
            return currentLevelDescriptor;
        }

        private int currentLevel = 0;
        private int lifes = InitialCountOfLifes;
        private bool sceneTransitionIsActive;

        private const int InitialCountOfLifes = 3;
        private const string RemainingLifesTextEntityName = "remaining-lifes-text";

        private SoundEffect explosionSoundEffect;
        
        private readonly ICollection<LevelDescriptor> levelDescriptors = new List<LevelDescriptor>();
    }
}
