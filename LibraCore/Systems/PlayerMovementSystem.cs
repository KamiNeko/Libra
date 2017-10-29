using LibraCore.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using System;

namespace LibraCore.Systems
{
    public class PlayerMovementSystem : EntityProcessingSystem
    {
        public PlayerMovementSystem() : 
            base(new Matcher().all(typeof(PlayerControllerComponent), typeof(BulletControllerComponent)))
        {
        }

        public override void process(Entity entity)
        {
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
                bulletControllerComponent.ShouldCreateBullet = true;
            }

            var playerControllerComponent = entity.getComponent<PlayerControllerComponent>();
            entity.position += moveDir * playerControllerComponent.Speed * Time.deltaTime;
        }
    }
}
