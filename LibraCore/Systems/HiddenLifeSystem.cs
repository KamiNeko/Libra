using LibraCore.Components;
using LibraCore.Scenes;
using Nez;

namespace LibraCore.Systems
{
    public class HiddenLifeSystem : EntityProcessingSystem
    {
        public HiddenLifeSystem() : base(new Matcher().all(typeof(HiddenLifeComponent), typeof(CollisionCheckComponent)))
        {
        }

        public override void process(Entity entity)
        {
            var hiddenLifeComponent = entity.getComponent<HiddenLifeComponent>();
            var collisionCheckComponent = entity.getComponent<CollisionCheckComponent>();
            
            if (collisionCheckComponent.HasCollisions)
            {
                bool collisionWithPlayerDetected = false;

                foreach (var collidedEntity in collisionCheckComponent.HasCollisionsWith)
                {
                    var playerControllerComponent = collidedEntity.getComponent<PlayerControllerComponent>();
                    if (playerControllerComponent != null)
                    {
                        collidedEntity.getComponent<CollisionCheckComponent>().HasCollisions = false;
                        collidedEntity.getComponent<CollisionCheckComponent>().HasCollisionsWith.Remove(entity);
                        collisionWithPlayerDetected = true;
                    }
                }

                if (collisionWithPlayerDetected)
                {
                    (scene as LevelScene).Lifes += hiddenLifeComponent.Lifes;
                    (scene as LevelScene).RecreateRemainingLifesText();
                    collisionCheckComponent.HasCollisions = false;
                    collisionCheckComponent.HasCollisionsWith.Clear();

                    entity.detachFromScene();
                }
            }
        }
    }
}
