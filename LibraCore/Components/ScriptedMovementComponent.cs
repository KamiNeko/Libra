using Microsoft.Xna.Framework;
using Nez;

namespace LibraCore.Components
{
    public class ScriptedMovementComponent : Component
    {
        public float SpeedInPixelsPerSecond { get; set; }
        public Vector2[] Paths { get; set; }
        public int CurrentPathIndex { get; set; }
    }
}
