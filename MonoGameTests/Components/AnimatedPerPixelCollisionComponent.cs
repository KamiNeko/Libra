using System;
using System.Collections.Generic;
using Nez.Sprites;
using Microsoft.Xna.Framework;

namespace MonoGameTests.Components
{
    internal class AnimatedPerPixelCollisionComponent<TEnum> : PerPixelCollisionComponent where TEnum : struct, IComparable, IFormattable
    {
        public AnimatedPerPixelCollisionComponent(Sprite<TEnum> animatedSprite) : base(animatedSprite)
        {
            this.animatedSprite = animatedSprite ?? throw new ArgumentNullException(nameof(animatedSprite));
            animatedTextureWidths = new Dictionary<TEnum, IList<int>>();
            animatedBooleanCollisionMap = new Dictionary<TEnum, IList<bool[]>>();
        }
        
        protected override bool[] BooleanCollisionMap
        {
            get
            {
                var currentAnimation = animatedSprite.CurrentAnimation;
                var currentFrameIndex = animatedSprite.CurrentFrame;

                if (!animatedBooleanCollisionMap.ContainsKey(currentAnimation))
                {
                    return null;
                }

                return animatedBooleanCollisionMap[currentAnimation][currentFrameIndex];
            }
        }

        protected override int TextureWidth
        {
            get
            {
                var currentAnimation = animatedSprite.CurrentAnimation;
                var currentFrameIndex = animatedSprite.CurrentFrame;
                return animatedTextureWidths[currentAnimation][currentFrameIndex];
            }
        }
        
        protected override void InitializeCollisionMap()
        {
            var animationKeys = animatedSprite.AnimationKeys;

            foreach (var animationKey in animationKeys)
            {
                InitializeCollisionMapForAnimationKey(animationKey);
            }
        }

        private void InitializeCollisionMapForAnimationKey(TEnum animationKey)
        {
            var animation = animatedSprite.GetAnimation(animationKey);
            var countAnimationFrames = animation.frames.Count;

            animatedBooleanCollisionMap[animationKey] = new List<bool[]>();
            animatedTextureWidths[animationKey] = new List<int>();

            foreach (var animationSubtexture in animation.frames)
            {
                var animationSpriteTexture = animationSubtexture.texture2D;
                var animationTextureBounds = animationSubtexture.sourceRect;

                var countPixels = animationTextureBounds.Width * animationTextureBounds.Height;

                var collisionMap = new Color[countPixels];
                animationSpriteTexture.GetData(0, animationTextureBounds, collisionMap, 0, 1);

                animatedTextureWidths[animationKey].Add(animationSubtexture.sourceRect.Width);

                var booleanCollisionMap = new bool[countPixels];

                for (int i = 0; i < countPixels; ++i)
                {
                    booleanCollisionMap[i] = (collisionMap[i].A > 0);
                }

                animatedBooleanCollisionMap[animationKey].Add(booleanCollisionMap);
            }
        }

        private Sprite<TEnum> animatedSprite;
        private IDictionary<TEnum, IList<int>> animatedTextureWidths;
        private IDictionary<TEnum, IList<bool[]>> animatedBooleanCollisionMap;
    }
}
