using Nez;
using Nez.Sprites;

namespace MonoGameTests.Components
{
    internal class EntityBoundsOutOfScreenTesterComponent : Component, IUpdatable
    {
        public bool EntityBoundsOutOfScreen { get; set; }

        public void update()
        {
            var sprite = Entity.getComponent<Sprite>();

            var bounds = sprite.Bounds;

            const float Left = 0.0f;
            const float Right = 640.0f;
            const float Top = 0.0f;
            const float Bottom = 480.0f;

            if (bounds.Left < Left || bounds.Right > Right ||
                bounds.Top < Top || bounds.Bottom > Bottom)
            {
                EntityBoundsOutOfScreen = true;
            }
            else
            {
                EntityBoundsOutOfScreen = false;
            }
        }
    }
}
