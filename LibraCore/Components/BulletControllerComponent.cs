using Microsoft.Xna.Framework;
using Nez;
using System;

namespace LibraCore.Components
{
    public class BulletControllerComponent : Component
    {
        public bool ShouldCreateBullet { get; set; }

        public float Speed { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Offset { get; set; }

        public DateTime LastBulletShootTimestamp { get; set; }
        public TimeSpan BulletCooldown { get; set; } = TimeSpan.FromMilliseconds(500);

        public Entity Bullet { get; set; }
    }
}
