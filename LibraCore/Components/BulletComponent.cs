using Microsoft.Xna.Framework;
using Nez;

namespace LibraCore.Components
{
    public class BulletComponent : Component
    {
        public float Speed { get; set; }
        public Vector2 Direction { get; set; }
        public Entity BulletShootingEntity { get; set; }
    }
}
