﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTests.Components;
using Nez;
using Nez.Sprites;
using Nez.Systems;
using Nez.Textures;
using System;
using System.Collections.Generic;

namespace MonoGameTests.Levels
{
    internal class LevelLoader
    {
        public LevelLoader(NezContentManager contentManager, LevelDescriptor levelDescriptor)
        {
            this.contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
            this.levelDescriptor = levelDescriptor ?? throw new ArgumentNullException(nameof(levelDescriptor));
        }

        public IEnumerable<Entity> CreateEntites()
        {
            var entites = new List<Entity>();

            entites.Add(CreateSpaceshipEntity());
            
            foreach (var entityDescriptor in levelDescriptor.EntityDescriptors)
            {
                entites.Add(CreateEntity(entityDescriptor));
            }

            return entites;
        }

        private Entity CreateSpaceshipEntity()
        {
            var entity = new Entity("space-ship");
            var sprite = new Sprite(contentManager.Load<Texture2D>(SpaceshipTextureName));
            sprite.RenderLayer = 100;

            entity.addComponent(sprite);
            entity.addComponent(CreateSpaceshipDriveSprite());
            entity.addComponent(new PlayerMovable());
            entity.addComponent(new PerPixelCollisionComponent(sprite));
            entity.addComponent(new CollisionTesterComponent());
            entity.addComponent(new EntityBoundsOutOfScreenTesterComponent());

            entity.transform.setPosition(levelDescriptor.StartPosition);
            return entity;
        }

        private Sprite CreateSpaceshipDriveSprite()
        {
            var spaceshipDriveTexture = contentManager.Load<Texture2D>(SpaceshipDriveTextureName);
            var spaceshipDriveSubtextures = Subtexture.subtexturesFromAtlas(spaceshipDriveTexture, 32, 32);

            var fireAnimationSprite = new Sprite<int>(spaceshipDriveSubtextures[0]);
            fireAnimationSprite.SetRenderLayer(100);
            fireAnimationSprite.SetLocalOffset(new Vector2(1f, 28f));

            fireAnimationSprite.AddAnimation(0, new SpriteAnimation(new List<Subtexture>()
            {
                spaceshipDriveSubtextures[0],
                spaceshipDriveSubtextures[1],
                spaceshipDriveSubtextures[2],
                spaceshipDriveSubtextures[3]
            }));

            fireAnimationSprite.Play(0);

            return fireAnimationSprite;
        }

        private Entity CreateEntity(EntityDescriptor entityDescriptor)
        {
            var entity = new Entity(entityDescriptor.EntityName);

            if (entityDescriptor.AnimationDescriptor.Active)
            {
                var texture = contentManager.Load<Texture2D>(entityDescriptor.TextureName);
                var subtextures = Subtexture.subtexturesFromAtlas(texture, entityDescriptor.AnimationDescriptor.SubtextureWidth, entityDescriptor.AnimationDescriptor.SubtextureHeight);

                var animationStartFrame = entityDescriptor.AnimationDescriptor.StartAnimationFrame;
                var animationCountFrames = (entityDescriptor.AnimationDescriptor.StopAnimationFrame - entityDescriptor.AnimationDescriptor.StartAnimationFrame);

                var animatedSprite = new Sprite<int>(subtextures[0]);
                animatedSprite.SetRenderLayer(200);

                var animation = new SpriteAnimation(new List<Subtexture>(subtextures.GetRange(animationStartFrame, animationCountFrames)))
                {
                    fps = entityDescriptor.AnimationDescriptor.AnimationFramesPerSecond
                };

                animatedSprite.AddAnimation(0, animation);
                animatedSprite.Play(0);

                if (entityDescriptor.IsCollidable)
                {
                    entity.addComponent(new AnimatedPerPixelCollisionComponent<int>(animatedSprite));
                }

                entity.addComponent(animatedSprite);
            }
            else
            {
                var sprite = new Sprite(contentManager.Load<Texture2D>(entityDescriptor.TextureName));
                sprite.SetRenderLayer(200);

                if (entityDescriptor.IsCollidable)
                {
                    entity.addComponent(new PerPixelCollisionComponent(sprite));
                }

                entity.addComponent(sprite);
            }

            entity.transform.setPosition(entityDescriptor.Position);

            return entity;
        }

        private readonly NezContentManager contentManager;
        private readonly LevelDescriptor levelDescriptor;

        private const string SpaceshipTextureName = "space-ship";
        private const string SpaceshipDriveTextureName = "space-ship-fire";
    }
}