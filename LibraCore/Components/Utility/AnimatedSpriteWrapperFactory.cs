using Nez.Sprites;
using System;

namespace LibraCore.Components.Utility
{
    public class AnimatedSpriteWrapperFactory
    {
        public IAnimatedSprite Create(Sprite sprite)
        {
            return new StaticSpriteWrapper(sprite);
        }

        public IAnimatedSprite Create<TEnum>(Sprite<TEnum> sprite) where TEnum : struct, IComparable, IFormattable
        {
            return new AnimatedSpriteWrapper<TEnum>(sprite);
        }
    }
}
