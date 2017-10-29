using LibraCore.Components;
using Nez;
using Nez.Sprites;

namespace LibraCore.Systems
{
    public class EntityOutOfLevelBoundsTesterSystem : EntityProcessingSystem
    {
        public EntityOutOfLevelBoundsTesterSystem() : base(new Matcher().all(
                typeof(EntityOutOfLevelBoundsTesterComponent),
                typeof(Sprite)))
        {
        }

        public override void process(Entity entity)
        {
            var sprite = entity.getComponent<Sprite>();
            var bounds = sprite.Bounds;

            const float Left = 0.0f;
            const float Right = 640.0f;
            const float Top = 0.0f;
            const float Bottom = 480.0f;

            var entityOutOfLevelBoundsTesterComponent = entity.getComponent<EntityOutOfLevelBoundsTesterComponent>();

            if (bounds.Left < Left || bounds.Right > Right || bounds.Top < Top || bounds.Bottom > Bottom)
            {
                entityOutOfLevelBoundsTesterComponent.OutOfBounds = true;
            }
            else
            {
                entityOutOfLevelBoundsTesterComponent.OutOfBounds = false;
            }
        }
    }
}
