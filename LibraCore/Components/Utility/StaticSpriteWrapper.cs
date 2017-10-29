using Nez.Sprites;
using System;
using Nez;
using Nez.Textures;
using System.Collections.Generic;

namespace LibraCore.Components.Utility
{
    public class StaticSpriteWrapper : IAnimatedSprite
    {
        public StaticSpriteWrapper(Sprite sprite)
        {
            this.sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
        }

        public int CurrentAnimationIndex => 0;
        public int CurrentAnimationFrameIndex => 0;

        public int[] AnimatedIndices => new[] { 0 };

        public RectangleF Bounds => sprite.Bounds;

        public int GetCountFramesOfAnimationIndex(int animationIndex)
        {
            return 1;
        }

        public SpriteAnimation GetAnimation(int animationIndex)
        {
            return new SpriteAnimation { frames = new List<Subtexture> { sprite.Subtexture } };
        }

        private readonly Sprite sprite;
    }
}
