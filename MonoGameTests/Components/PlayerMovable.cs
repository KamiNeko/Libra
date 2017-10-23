using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace MonoGameTests.Components
{
    public class PlayerMovable : Component, IUpdatable
    {
        public float speed = 100f;

        public void update()
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
            
            Entity.position += moveDir * speed * Time.deltaTime;
        }
    }
}
