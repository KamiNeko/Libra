using LibraCore.Components;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System;

namespace LibraCore.Systems
{
    public class BulletControllerSystem : EntityProcessingSystem
    {
        public BulletControllerSystem() : base(new Matcher().all(typeof(BulletControllerComponent)))
        {
        }

        protected override void begin()
        {
            base.begin();
            shootSoundEffect = scene.ContentManager.Load<SoundEffect>("shoot");
        }

        public void Reset()
        {
            var iterationSafeEntitesCopy = _entities.ToArray();

            foreach (var bulletControllerEntity in iterationSafeEntitesCopy)
            {
                var bulletControllerComponent = bulletControllerEntity.getComponent<BulletControllerComponent>();
                bulletControllerComponent.Bullet = null;
                bulletControllerComponent.LastBulletShootTimestamp = DateTime.MinValue;
            }
        }

        public override void process(Entity entity)
        {
            var bulletControllerComponent = entity.getComponent<BulletControllerComponent>();

            if (bulletControllerComponent.ShouldCreateBullet)
            {
                TryToCreateBullet(entity, bulletControllerComponent);
                bulletControllerComponent.ShouldCreateBullet = false;
            }
        }

        private void TryToCreateBullet(Entity bulletShootingEntity, BulletControllerComponent bulletControllerComponent)
        {
            var bulletEntity = bulletControllerComponent.Bullet;
            var timeDifferenceToLastShoot = DateTime.Now - bulletControllerComponent.LastBulletShootTimestamp;

            if (bulletEntity == null && timeDifferenceToLastShoot > bulletControllerComponent.BulletCooldown)
            {
                bulletEntity = scene.CreateEntity(CreateBulletEntityName(bulletShootingEntity));
                var sprite = new Sprite(scene.ContentManager.Load<Texture2D>("bullet"));
                sprite.SetRenderLayer(200);

                bulletEntity.addComponent(new BulletComponent()
                {
                    Speed = bulletControllerComponent.Speed,
                    Direction = bulletControllerComponent.Direction,
                    BulletShootingEntity = bulletShootingEntity
                });
                bulletEntity.addComponent(new PerPixelCollisionComponent(sprite));
                bulletEntity.addComponent(new CollisionTesterComponent());
                bulletEntity.addComponent(new EntityOutOfLevelBoundsTesterComponent());
                bulletEntity.addComponent(sprite);

                bulletEntity.transform.setPosition(bulletShootingEntity.position + bulletControllerComponent.Offset);

                shootSoundEffect.CreateInstance().Play();

                bulletControllerComponent.LastBulletShootTimestamp = DateTime.Now;
                bulletControllerComponent.Bullet = bulletEntity;
            }
        }
        
        private string CreateBulletEntityName(Entity bulletShootingEntity)
        {
            var entityName = bulletShootingEntity.name;
            var bulletEntityName = $"{entityName}-bullet";

            return bulletEntityName;
        }

        private SoundEffect shootSoundEffect;
    }
}
