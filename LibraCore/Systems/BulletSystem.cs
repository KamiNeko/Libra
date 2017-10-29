using LibraCore.Components;
using Nez;

namespace LibraCore.Systems
{
    public class BulletSystem : EntityProcessingSystem
    {
        public BulletSystem() : base(new Matcher().all(typeof(BulletComponent)))
        {
        }

        public override void process(Entity entity)
        {
            var bulletComponent = entity.getComponent<BulletComponent>();
            entity.position += bulletComponent.Direction * bulletComponent.Speed * Time.deltaTime;
        }
    }
}
