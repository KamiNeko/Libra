using LibraCore.Components;
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

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            for (int i = 0; i < 10; ++i)
            {
                var state = GamePad.GetState((PlayerIndex)i);
                if (state.IsConnected)
                {
                    gamePadState = state;
                    break;
                }
            }
            
            if (Input.isKeyDown(Keys.Left) || gamePadState.ThumbSticks.Left.X < 0)
            {
                moveDir.X = -1f;
            }
            else if (Input.isKeyDown(Keys.Right) || gamePadState.ThumbSticks.Left.X > 0)
            {
                moveDir.X = 1f;
            }

            if (Input.isKeyDown(Keys.Up) || gamePadState.ThumbSticks.Left.Y > 0)
            {
                moveDir.Y = -1f;
            }
            else if (Input.isKeyDown(Keys.Down) || gamePadState.ThumbSticks.Left.Y < 0)
            {
                moveDir.Y = 1f;
            }

            if (Input.isKeyDown(Keys.Space) || gamePadState.Buttons.X == ButtonState.Pressed)
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

            var playerControllerComponent = entity.getComponent<PlayerControllerComponent>();
            entity.position += moveDir * playerControllerComponent.Speed * Time.deltaTime;
        }

        private DateTime frozenTimestamp;
        private TimeSpan frozenDuration;
        private bool frozen;
    }
}
