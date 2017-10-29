using Microsoft.Xna.Framework;

namespace LibraCore.LevelBuilding
{
    public class MovementDescriptor
    {
        public bool Active { get; set; }
        public Vector2[] Positions { get; set; }
        public float Speed { get; set; }
    }
}
