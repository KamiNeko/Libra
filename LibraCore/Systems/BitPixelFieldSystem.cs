using LibraCore.Components;
using Microsoft.Xna.Framework;
using Nez;
using System.Collections.Generic;

namespace LibraCore.Systems
{
    public class BitPixelFieldSystem : EntityProcessingSystem
    {
        public BitPixelFieldSystem() : base(new Matcher().one(typeof(BitPixelFieldComponent)))
        {
        }

        public override void onAdded(Entity entity)
        {
            base.onAdded(entity);
            CreatePixelField(entity);
        }

        private void CreatePixelField(Entity entity)
        {
            var pixelFieldComponent = entity.getComponent<BitPixelFieldComponent>();
            var animationKeys = pixelFieldComponent.AnimatedSprite.AnimatedIndices;

            foreach (var animationKey in animationKeys)
            {
                InitializeCollisionMapForAnimationKey(pixelFieldComponent, animationKey);
            }
        }

        private void InitializeCollisionMapForAnimationKey(BitPixelFieldComponent bitPixelFieldComponent, int animationKey)
        {
            var animatedSprite = bitPixelFieldComponent.AnimatedSprite;
            var animation = animatedSprite.GetAnimation(animationKey);
            var countAnimationFrames = animation.frames.Count;

            bitPixelFieldComponent.BitPixelsPerAnimationMap[animationKey] = new List<bool[]>();
            bitPixelFieldComponent.TextureWidthPerAnimationMap[animationKey] = new List<int>();

            foreach (var animationSubtexture in animation.frames)
            {
                var animationSpriteTexture = animationSubtexture.texture2D;
                var animationTextureBounds = animationSubtexture.sourceRect;

                var countPixels = animationTextureBounds.Width * animationTextureBounds.Height;

                var colorField = new Color[countPixels];
                animationSpriteTexture.GetData(0, animationTextureBounds, colorField, 0, 1);

                bitPixelFieldComponent.TextureWidthPerAnimationMap[animationKey].Add(animationSubtexture.sourceRect.Width);

                var bitField = new bool[countPixels];

                for (int i = 0; i < countPixels; ++i)
                {
                    bitField[i] = (colorField[i].A > 0);
                }

                bitPixelFieldComponent.BitPixelsPerAnimationMap[animationKey].Add(bitField);
            }
        }

        public override void process(Entity entity)
        {
        }
    }
}
