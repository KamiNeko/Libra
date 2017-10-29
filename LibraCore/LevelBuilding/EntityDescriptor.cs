using Microsoft.Xna.Framework;

namespace LibraCore.LevelBuilding
{
    public class EntityDescriptor
    {
        public EntityDescriptor()
        {
            AnimationDescriptor = new AnimationDescriptor { Active = false };
        }

        public string EntityName { get; set; }
        public Vector2 Position { get; set; }

        public bool IsCollidable { get; set; }
        public string TextureName { get; set; }

        public AnimationDescriptor AnimationDescriptor { get; set; }
        public MovementDescriptor MovementDescriptor { get; set; }
    }
}
