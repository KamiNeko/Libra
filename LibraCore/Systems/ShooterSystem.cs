using LibraCore.Components;
using Nez;

namespace LibraCore.Systems
{
    public class ShooterSystem : EntityProcessingSystem
    {
        public ShooterSystem() : base(new Matcher().all(
            typeof(ShooterComponent),
            typeof(BulletControllerComponent)))
        {
        }

        public override void process(Entity entity)
        {
            var bulletControllerComponent = entity.getComponent<BulletControllerComponent>();
            bulletControllerComponent.ShouldCreateBullet = true;

            var shooterComponent = entity.getComponent<ShooterComponent>();
            if (shooterComponent.Health <= 0)
            {
                entity.detachFromScene();
            }
        }
    }
}
