using Nez;
using System.Collections.Generic;

namespace LibraCore.Components
{
    internal class CollisionCheckComponent : Component
    {
        public CollisionCheckComponent()
        {
            HasCollisionsWith = new List<Entity>();
        }

        public bool HasCollisions { get; set; }
        public ICollection<Entity> HasCollisionsWith { get; set; }
    }
}
