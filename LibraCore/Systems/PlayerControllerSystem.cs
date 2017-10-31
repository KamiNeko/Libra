﻿using LibraCore.Components;
using LibraCore.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using System;

namespace LibraCore.Systems
{
    public class PlayerControllerSystem : EntityProcessingSystem
    {
        public PlayerControllerSystem() : base(new Matcher().all
                (typeof(PlayerControllerComponent)))
        {
        }

        public void Freeze(TimeSpan duration)
        {
            if (frozen)
            {
                frozenDuration += duration;
                return;
            }

            frozenTimestamp = DateTime.Now;
            frozenDuration = duration;
            frozen = true;
        }

        public override void process(Entity entity)
        {
            if (frozen && DateTime.Now - frozenTimestamp < frozenDuration)
            {
                return;
            }
            else if (frozen)
            {
                frozen = false;
            }

            var moveDir = Vector2.Zero;

            if (Input.isKeyDown(Keys.Left))
            {
                moveDir.X = -1f;
            }
            else if (Input.isKeyDown(Keys.Right))
            {
                moveDir.X = 1f;
            }

            if (Input.isKeyDown(Keys.Up))
            {
                moveDir.Y = -1f;
            }
            else if (Input.isKeyDown(Keys.Down))
            {
                moveDir.Y = 1f;
            }

            if (Input.isKeyDown(Keys.Space))
            {
                var bulletControllerComponent = entity.getComponent<BulletControllerComponent>();

                if (bulletControllerComponent != null)
                {
                    bulletControllerComponent.ShouldCreateBullet = true;
                }
            }

            if (Input.isKeyReleased(Keys.Q))
            {
                ((LevelScene)scene).LevelEditorModeActive = !((LevelScene)scene).LevelEditorModeActive;
            }

            if (Input.isKeyReleased(Keys.F))
            {
                Screen.isFullscreen = !Screen.isFullscreen;
                Screen.applyChanges();
            }

            var playerControllerComponent = entity.getComponent<PlayerControllerComponent>();
            entity.position += moveDir * playerControllerComponent.Speed * Time.deltaTime;
        }

        private DateTime frozenTimestamp;
        private TimeSpan frozenDuration;
        private bool frozen;
    }
}
