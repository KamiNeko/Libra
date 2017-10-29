﻿using LibraCore.Components;
using Nez;

namespace LibraCore.Systems
{
    public class BulletSystem : EntityProcessingSystem
    {
        public BulletSystem() : base(new Matcher().all(
            typeof(BulletComponent),
            typeof(CollisionTesterComponent),
            typeof(EntityBoundsOutOfScreenTesterComponent)))
        {
        }

        public void Reset()
        {
            var iterationSafeEntitesCopy = _entities.ToArray();

            foreach (var bulletEntity in iterationSafeEntitesCopy)
            {
                bulletEntity.detachFromScene();
            }
        }

        public override void process(Entity entity)
        {
            var bulletComponent = entity.getComponent<BulletComponent>();
            entity.position += bulletComponent.Direction * bulletComponent.Speed * Time.deltaTime;

            var collisionTesterComponent = entity.getComponent<CollisionTesterComponent>();
            var entityBoundsOutOfScreenTesterComponent = entity.getComponent<EntityBoundsOutOfScreenTesterComponent>();

            if (collisionTesterComponent.HasCollisions || entityBoundsOutOfScreenTesterComponent.EntityBoundsOutOfScreen)
            {
                var bulletShootingEntity = bulletComponent.BulletShootingEntity;
                var bulletControllerComponent = bulletShootingEntity.getComponent<BulletControllerComponent>();
                bulletControllerComponent.Bullet = null;

                entity.detachFromScene();
            }
        }
    }
}
