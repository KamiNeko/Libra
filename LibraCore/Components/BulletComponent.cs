using System;
using Microsoft.Xna.Framework;
using Nez;

namespace LibraCore.Components
{
    public class BulletComponent : Component, IUpdatable
    {
        public float Speed { get; set; }
        public Vector2 Direction { get; set; }

        public void update()
        {
            Entity.position += Direction * Speed * Time.deltaTime;
        }
    }
}
