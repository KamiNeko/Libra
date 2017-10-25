using Nez;
using System;

namespace LibraCore.Components
{
    internal class CollisionTesterComponent : Component, IUpdatable
    {
        public bool HasCollisions { get; set; }

        public void update()
        {
            HasCollisions = HasCollisionsWithOtherEntites();
        }

        private bool HasCollisionsWithOtherEntites()
        {
            var ownCollisionComponent = Entity.getComponent<PerPixelCollisionComponent>();

            if (ownCollisionComponent == null)
            {
                throw new NotSupportedException("The associated entity must have a PerPixelCollisionComponent component in order for this component to work correctly");
            }

            var collisionElements = Entity.scene.FindComponentsOfType<PerPixelCollisionComponent>();

            foreach (var collisionElement in collisionElements)
            {
                if (collisionElement != ownCollisionComponent)
                {
                    if (ownCollisionComponent.HasCollisionWith(collisionElement))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
