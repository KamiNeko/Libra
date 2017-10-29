using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LibraCore.Components.Utility
{
    public class AnimatedSpriteWrapper<TEnum> : IAnimatedSprite where TEnum : struct, IComparable, IFormattable
    {
        public AnimatedSpriteWrapper(Sprite<TEnum> animatedSprite)
        {
            this.animatedSprite = animatedSprite ?? throw new ArgumentNullException(nameof(animatedSprite));

            var animationKeyIndices = new Dictionary<TEnum, int>();

            foreach (var animationKey in animatedSprite.AnimationKeys)
            {
                animationKeyIndices.Add(animationKey, AnimationEnumerationToIndex(animationKey));
            }

            animationKeys = animationKeyIndices.OrderBy(x => x.Value).Select(x => x.Value).ToArray();
            animationKeyTranslations = animationKeyIndices.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
        }

        public int CurrentAnimationIndex => AnimationEnumerationToIndex(animatedSprite.CurrentAnimation);
        public int CurrentAnimationFrameIndex => animatedSprite.CurrentFrame;

        public int[] AnimatedIndices => animationKeys;
        public RectangleF Bounds => animatedSprite.Bounds;

        public int GetCountFramesOfAnimationIndex(int animationIndex)
        {
            return GetAnimation(animationIndex).frames.Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int AnimationEnumerationToIndex(TEnum value)
        {
            return (int)((object)value);
        }

        public SpriteAnimation GetAnimation(int animationIndex)
        {
            return animatedSprite.GetAnimation(animationKeyTranslations[animationIndex]);
        }

        private readonly int[] animationKeys;
        private readonly TEnum[] animationKeyTranslations;
        private readonly Sprite<TEnum> animatedSprite;
    }
}
