using Microsoft.Xna.Framework;

namespace LibraCore.LevelBuilding
{
    public class EntityDescriptor
    {
        public EntityDescriptor()
        {
            AnimationDescriptor = new AnimationDescriptor { Active = false };
            MovementDescriptor = new MovementDescriptor { Active = false };
            LightningDescriptor = new LightningDescriptor { Active = false };
            AnimationLoopDescriptor = new AnimationLoopDescriptor { Active = false };
        }

        public string EntityName { get; set; }
        public Vector2 Position { get; set; }

        public bool IsCollidable { get; set; }
        public string TextureName { get; set; }
        public bool LevelEditorMovable { get; set; }

        public AnimationDescriptor AnimationDescriptor { get; set; }
        public MovementDescriptor MovementDescriptor { get; set; }
        public LightningDescriptor LightningDescriptor { get; set; }
        public AnimationLoopDescriptor AnimationLoopDescriptor { get; set; }
    }
}
