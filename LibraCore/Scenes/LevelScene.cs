using Microsoft.Xna.Framework;
using LibraCore.Components;
using LibraCore.LevelBuilding;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraCore.Scenes
{
    public class LevelScene : Scene
    {
        public bool LevelEditorModeActive { get; set; }

        public override void Initialize()
        {
            base.Initialize();

            AddRenderer(new DefaultRenderer());
            ClearColor = Color.Black;
            SetDesignResolution(640, 480, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);

            CreateLevelDescriptors();
            SwitchToNextLevel();
        }

        public override void Update()
        {
            base.Update();

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
                    var currentLevelDescriptor = GetCurrentLevelDescriptor();
                    spaceshipEntity.setPosition(currentLevelDescriptor.StartPosition);
                }
            }
            else if (LevelEditorModeActive)
            {
                spaceshipEntity.getComponent<Sprite>().Color = Color.White;
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
            levelDescriptors.Add(CreateSampleLevelDescriptor());
        }

        private LevelDescriptor CreateSampleLevelDescriptor()
        {
            var levelDescriptor = new LevelDescriptor { StartPosition = new Vector2(117, 458) };

            levelDescriptor.EntityDescriptors.Add(new EntityDescriptor
            {
                EntityName = "level",
                IsCollidable = true,
                Position = new Vector2(320, 240),
                TextureName = "level01",
                AnimationDescriptor = new AnimationDescriptor
                {
                    Active = false
                }
            });

            levelDescriptor.EntityDescriptors.Add(new EntityDescriptor
            {
                EntityName = "flower02",
                IsCollidable = true,
                Position = new Vector2(158f, 153),
                TextureName = "flower02",
                AnimationDescriptor = new AnimationDescriptor
                {
                    Active = true,
                    AnimationFramesPerSecond = 2,
                    StartAnimationFrame = 0,
                    StopAnimationFrame = 4,
                    SubtextureHeight = 64,
                    SubtextureWidth = 64
                }
            });

            return levelDescriptor;
        }

        private void SwitchToNextLevel()
        {
            UnloadAllEntites();

            currentLevel++;

            if (currentLevel > levelDescriptors.Count)
            {
                // TODO: Game over (with success)
                throw new NotImplementedException();
            }

            var currentLevelDescriptor = GetCurrentLevelDescriptor();

            var levelLoader = new LevelBuilder(ContentManager, currentLevelDescriptor);
            var entites = levelLoader.BuildEntites();

            foreach (var entity in entites)
            {
                AddEntity(entity);
            }
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

        private readonly ICollection<LevelDescriptor> levelDescriptors = new List<LevelDescriptor>();
    }
}
