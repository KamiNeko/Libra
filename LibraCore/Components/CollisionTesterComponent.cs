using Nez;
using System;

namespace LibraCore.Components
{
    internal class CollisionTesterComponent : Component, IUpdatable
    {
        public bool HasCollisions { get; set; }

        public void update()
        {
            CheckCollisionsWithOtherEntites();
        }

        private void CheckCollisionsWithOtherEntites()
        {
            var ownCollisionComponent = Entity.getComponent<PerPixelCollisionComponent>();

            if (ownCollisionComponent == null)
            {
                throw new NotSupportedException("The associated entity must have a PerPixelCollisionComponent component in order for this component to work correctly");
            }

            var collisionElements = Entity.scene.FindComponentsOfType<PerPixelCollisionComponent>();
            var collisionsFound = false;

            foreach (var collisionElement in collisionElements)
            {
                if (collisionElement != ownCollisionComponent)
                {
                    if (ownCollisionComponent.HasCollisionWith(collisionElement))
                    {
                        collisionsFound = true;

                        // NOTE: Due to random order in the update method, we make sure to update the counterpart of the collision
                        //  so that we have consistency in collision detection, if we check the other component
                        if (collisionElement.Entity.getComponent<CollisionTesterComponent>() != null)
                        {
                            collisionElement.Entity.getComponent<CollisionTesterComponent>().HasCollisions = true;
                        }
                    }
                }
            }

            HasCollisions = collisionsFound;
        }
    }
}
