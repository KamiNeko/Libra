using LibraCore.Components;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var bulletEntity = TryToFindBulletEntity(bulletShootingEntity);
            var timeDifferenceToLastShoot = DateTime.Now - bulletControllerComponent.LastBulletShootTimestamp;

            if (bulletEntity == null && timeDifferenceToLastShoot > bulletControllerComponent.BulletCooldown)
            {
                bulletEntity = scene.CreateEntity(GetBulletEntityName(bulletShootingEntity));
                var sprite = new Sprite(scene.ContentManager.Load<Texture2D>("bullet"));
                sprite.SetRenderLayer(200);

                bulletEntity.addComponent(new BulletComponent()
                {
                    Speed = 100f,
                    Direction = bulletControllerComponent.Direction
                });
                bulletEntity.addComponent(new PerPixelCollisionComponent(sprite));
                bulletEntity.addComponent(new CollisionTesterComponent());
                bulletEntity.addComponent(new EntityBoundsOutOfScreenTesterComponent());
                bulletEntity.addComponent(sprite);

                bulletEntity.transform.setPosition(bulletShootingEntity.position + bulletControllerComponent.Offset);

                shootSoundEffect.CreateInstance().Play();

                bulletControllerComponent.LastBulletShootTimestamp = DateTime.Now;
            }
        }

        private Entity TryToFindBulletEntity(Entity bulletShootingEntity)
        {
            var sceneEntites = scene.Entities;
            var bulletEntity = sceneEntites.findEntity(GetBulletEntityName(bulletShootingEntity));

            return bulletEntity;
        }

        private string GetBulletEntityName(Entity bulletShootingEntity)
        {
            var entityName = bulletShootingEntity.name;
            var bulletEntityName = $"{bulletShootingEntity}-bullet";

            return bulletEntityName;
        }

        private SoundEffect shootSoundEffect;
    }
}
