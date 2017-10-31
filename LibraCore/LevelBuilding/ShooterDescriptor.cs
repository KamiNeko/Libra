using Microsoft.Xna.Framework;
using System;

namespace LibraCore.LevelBuilding
{
    public class ShooterDescriptor
    {
        public bool Active { get; set; }
        public float Speed { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Offset { get; set; }
        public float BulletCooldownInSeconds { get; set; }
    }
}
