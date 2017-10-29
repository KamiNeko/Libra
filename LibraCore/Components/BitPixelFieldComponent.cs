using System;
using System.Collections.Generic;
using Nez;
using LibraCore.Components.Utility;

namespace LibraCore.Components
{
    internal class BitPixelFieldComponent : Component
    {
        public BitPixelFieldComponent(IAnimatedSprite animatedSprite)
        {
            AnimatedSprite = animatedSprite ?? throw new ArgumentNullException(nameof(animatedSprite));
            TextureWidthPerAnimationMap = new Dictionary<int, IList<int>>();
            BitPixelsPerAnimationMap = new Dictionary<int, IList<bool[]>>();
        }

        public RectangleF AnimatedSpriteBounds
        {
            get
            {
                return AnimatedSprite.Bounds;
            }
        }

        public bool[] PixelField
        {
            get
            {
                var currentAnimation = AnimatedSprite.CurrentAnimationIndex;
                var currentFrameIndex = AnimatedSprite.CurrentAnimationFrameIndex;

                if (!BitPixelsPerAnimationMap.ContainsKey(currentAnimation))
                {
                    return null;
                }

                return BitPixelsPerAnimationMap[currentAnimation][currentFrameIndex];
            }
        }

        public int TextureWidth
        {
            get
            {
                var currentAnimation = AnimatedSprite.CurrentAnimationIndex;
                var currentFrameIndex = AnimatedSprite.CurrentAnimationFrameIndex;
                return TextureWidthPerAnimationMap[currentAnimation][currentFrameIndex];
            }
        }

        public IAnimatedSprite AnimatedSprite { get; set; }
        public IDictionary<int, IList<int>> TextureWidthPerAnimationMap { get; set; }
        public IDictionary<int, IList<bool[]>> BitPixelsPerAnimationMap { get; set; }
    }
}
