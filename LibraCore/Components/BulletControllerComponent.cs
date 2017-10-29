using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System;

namespace LibraCore.Components
{
    public class BulletControllerComponent : Component
    {
        public bool ShouldCreateBullet { get => requestedToCreateBullet; set => requestedToCreateBullet = value; }
        public Vector2 Direction { get => direction; set => direction = value; }
        public Vector2 Offset { get => offset; set => offset = value; }

        public override void OnAddedToEntity()
        {
            shootSoundEffect = Entity.scene.ContentManager.Load<SoundEffect>("shoot");
        }

        private void TryToCreateBullet()
        {
            var bulletEntity = TryToFindBulletEntity();
            var timeDifferenceToLastShoot = DateTime.Now - lastBulletShootTimestamp;

            if (bulletEntity == null && timeDifferenceToLastShoot > TimeSpan.FromMilliseconds(500))
            {
                bulletEntity = Entity.scene.CreateEntity(GetBulletEntityName());
                var sprite = new Sprite(Entity.scene.ContentManager.Load<Texture2D>("bullet"));
                sprite.SetRenderLayer(200);

                bulletEntity.addComponent(new BulletComponent() { Speed = 100f, Direction = direction });
                bulletEntity.addComponent(new PerPixelCollisionComponent(sprite));
                bulletEntity.addComponent(new CollisionTesterComponent());
                bulletEntity.addComponent(new EntityBoundsOutOfScreenTesterComponent());
                bulletEntity.addComponent(sprite);
                
                bulletEntity.transform.setPosition(Entity.position + offset);

                shootSoundEffect.CreateInstance().Play();

                lastBulletShootTimestamp = DateTime.Now;
            }
        }
        
        private Entity TryToFindBulletEntity()
        {
            var sceneEntites = Entity.scene.Entities;
            var bulletEntity = sceneEntites.findEntity(GetBulletEntityName());

            return bulletEntity;
        }

        private string GetBulletEntityName()
        {
            var entityName = Entity.name;
            var bulletEntityName = $"{Entity}-bullet";

            return bulletEntityName;
        }

        private SoundEffect shootSoundEffect;
        private DateTime lastBulletShootTimestamp;

        private bool requestedToCreateBullet;
        private Vector2 direction;
        private Vector2 offset;
    }
}
