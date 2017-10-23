using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using System;

namespace MonoGameTests.Components
{
    internal class PerPixelCollisionComponent : Component
    {
        public PerPixelCollisionComponent(Sprite sprite)
        {
            this.sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
        }
        
        protected virtual bool[] BooleanCollisionMap
        {
            get
            {
                return booleanCollisionMap;
            }
        }

        protected RectangleF Bounds
        {
            get
            {
                return sprite.Bounds;
            }
        }

        protected virtual int TextureWidth
        {
            get
            {
                return textureWidth;
            }
        }
        
        public override void OnAddedToEntity()
        {
            InitializeCollisionMap();
        }

        protected virtual void InitializeCollisionMap()
        {
            var spriteSubtexture = sprite.Subtexture;
            var spriteTexture = spriteSubtexture.texture2D;
            var textureBounds = spriteSubtexture.sourceRect;

            var countPixels = textureBounds.Width * textureBounds.Height;

            var collisionMap = new Color[countPixels];
            spriteTexture.GetData(0, textureBounds, collisionMap, 0, 1);

            textureWidth = sprite.Subtexture.sourceRect.Width;

            booleanCollisionMap = new bool[countPixels];

            for (int i = 0; i < countPixels; ++i)
            {
                booleanCollisionMap[i] = (collisionMap[i].A > 0);
            }
        }

        public bool HasCollisionWith(PerPixelCollisionComponent component)
        {
            if (BooleanCollisionMap == null || component.BooleanCollisionMap == null)
            {
                return false;
            }

            var ownBounds = Bounds;
            var componentBounds = component.Bounds;

            int left = (int)Math.Max(ownBounds.Left, componentBounds.Left);
            int top = (int)Math.Max(ownBounds.Top, componentBounds.Top);
            int width = (int)Math.Min(ownBounds.Right, componentBounds.Right) - left;
            int height = (int)Math.Min(ownBounds.Bottom, componentBounds.Bottom) - top;

            var intersectingRectangle = new Rectangle(left, top, width, height);

            var ownTextureWidth = TextureWidth;
            var componentTextureWidth = component.TextureWidth;

            for (var x = intersectingRectangle.Left; x < intersectingRectangle.Right; x++)
            {
                for (var y = intersectingRectangle.Top; y < intersectingRectangle.Bottom; y++)
                {
                    var sourceColor = BooleanCollisionMap[(x - (int)ownBounds.Left) + (y - (int)ownBounds.Top) * ownTextureWidth];
                    var targetColor = component.BooleanCollisionMap[(x - (int)componentBounds.Left) + (y - (int)componentBounds.Top) * (int)componentTextureWidth];

                    if (sourceColor && targetColor)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected int textureWidth;
        private bool[] booleanCollisionMap;

        private readonly Sprite sprite;
    }
}
