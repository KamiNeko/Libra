using Microsoft.Xna.Framework;
using LibraCore.Components;
using LibraCore.LevelBuilding;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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

            CreateLevelDescriptors();
            SwitchToNextLevel();
            CreateRemainingLifesText();

            shootSoundEffect = ContentManager.Load<SoundEffect>("shoot");
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
                        HandleShipCollision(spaceshipEntity);
                    }
                }
                else if (LevelEditorModeActive)
                {
                    spaceshipEntity.getComponent<Sprite>().Color = Color.White;
                }

                var shipBulletEntity = Entities.findEntity("player-bullet");

                if (shipBulletEntity != null)
                {
                    var component = shipBulletEntity.getComponent<CollisionTesterComponent>();
                    if (component.HasCollisions)
                    {
                        shipBulletEntity.detachFromScene();
                    }
                }

                if (Input.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                {
                    if (shipBulletEntity == null)
                    {
                        var entity = CreateEntity("player-bullet");
                        var sprite = new Sprite(ContentManager.Load<Texture2D>("bullet"));
                        sprite.SetRenderLayer(200);

                        entity.addComponent(new BulletComponent() { Speed = 100f, Direction = new Vector2(0f, 1f) });
                        entity.addComponent(new PerPixelCollisionComponent(sprite));
                        entity.addComponent(new CollisionTesterComponent());                        
                        entity.addComponent(sprite);

                        var offset = new Vector2(0, 32f);
                        entity.transform.setPosition(spaceshipEntity.position + offset);

                        shootSoundEffect.CreateInstance().Play();
                    }
                }
            }
        }

        private void HandleShipCollision(Entity spaceshipEntity)
        {
            lifes--;

            if (lifes <= 0)
            {
                HandleGameOver();
            }
            else
            {
                var transition = new FadeTransition() { fadeInDuration = 0.2f, fadeOutDuration = 0.2f, delayBeforeFadeInDuration = 0.0f };
                var currentLevelDescriptor = GetCurrentLevelDescriptor();

                transition.onScreenObscured = () =>
                {
                    spaceshipEntity.setPosition(currentLevelDescriptor.StartPosition);
                };

                transition.onTransitionCompleted = () =>
                {
                    sceneTransitionIsActive = false;
                };

                Core.startSceneTransition(transition);

                RecreateRemainingLifesText();

                sceneTransitionIsActive = true;
            }
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
            // TODO: Filter by level entities, otherwise this throws the UI out
            UnloadAllEntites();

            currentLevel++;

            if (currentLevel > levelDescriptors.Count)
            {
                HandleGameWon();
            }
            else
            {
                var currentLevelDescriptor = GetCurrentLevelDescriptor();

                var levelLoader = new LevelBuilder(ContentManager, currentLevelDescriptor);
                var entites = levelLoader.BuildEntites();

                foreach (var entity in entites)
                {
                    AddEntity(entity);
                }
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

        private SoundEffect shootSoundEffect;

        private readonly ICollection<LevelDescriptor> levelDescriptors = new List<LevelDescriptor>();
    }
}
