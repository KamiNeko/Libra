namespace LibraCore.LevelBuilding
{
    public class AnimationDescriptor
    {
        public bool Active { get; set; }

        public int StartAnimationFrame { get; set; }
        public int StopAnimationFrame { get; set; }
        public int AnimationFramesPerSecond { get; set; }

        public int SubtextureWidth { get; set; }
        public int SubtextureHeight { get; set; }
    }
}
