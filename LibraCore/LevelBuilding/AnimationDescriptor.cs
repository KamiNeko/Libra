using System.Collections.Generic;

namespace LibraCore.LevelBuilding
{
    public class AnimationDescriptor
    {
        public bool Active { get; set; }
        public int AnimationFramesPerSecond { get; set; }

        public int SubtextureWidth { get; set; }
        public int SubtextureHeight { get; set; }

        public List<AnimationFrameSetDescriptor> AnimationFrameSets { get; set; }
    }
}
