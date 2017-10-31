using Microsoft.Xna.Framework;

namespace LibraCore.LevelBuilding
{
    public class EntityDescriptor
    {
        public EntityDescriptor()
        {
            AnimationDescriptor = new AnimationDescriptor { Active = false };
            MovementDescriptor = new MovementDescriptor { Active = false };
            PeriodicVisibilityToggleDescriptor = new PeriodicVisibilityToggleDescriptor { Active = false };
            AnimationLoopDescriptor = new AnimationLoopDescriptor { Active = false };
            ShooterDescriptor = new ShooterDescriptor { Active = false };
            DoorDescriptor = new DoorDescriptor { Active = false };
            HiddenLifeDescriptor = new HiddenLifeDescriptor { Active = false };
        }

        public string EntityName { get; set; }
        public Vector2 Position { get; set; }

        public bool IsCollidable { get; set; }
        public string TextureName { get; set; }

        public AnimationDescriptor AnimationDescriptor { get; set; }
        public MovementDescriptor MovementDescriptor { get; set; }
        public PeriodicVisibilityToggleDescriptor PeriodicVisibilityToggleDescriptor { get; set; }
        public AnimationLoopDescriptor AnimationLoopDescriptor { get; set; }
        public ShooterDescriptor ShooterDescriptor { get; set; }
        public DoorDescriptor DoorDescriptor { get; set; }
        public HiddenLifeDescriptor HiddenLifeDescriptor { get; set; }
    }
}
