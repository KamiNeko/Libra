using Nez;
using Nez.Sprites;

namespace LibraCore.Components.Utility
{
    public interface IAnimatedSprite
    {
        RectangleF Bounds { get; }

        int CurrentAnimationIndex { get; }
        int CurrentAnimationFrameIndex { get; }

        int[] AnimatedIndices { get; }
        int GetCountFramesOfAnimationIndex(int animationIndex);

        SpriteAnimation GetAnimation(int animationIndex);
    }
}
