using LibraCore.Components;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraCore.Systems
{
    public class CollisionCheckSystem : EntityProcessingSystem
    {
        public CollisionCheckSystem() : base(new Matcher().all(typeof(BitPixelFieldComponent), typeof(CollisionCheckComponent)))
        {
        }

        public override void process(Entity entity)
        {
            var bitPixelFieldComponent = entity.getComponent<BitPixelFieldComponent>();

            var otherBitPixelFieldComponents = scene.FindComponentsOfType<BitPixelFieldComponent>();
            var collisionsFound = false;

            foreach (var otherBitPixelFieldComponent in otherBitPixelFieldComponents)
            {
                if (otherBitPixelFieldComponent != bitPixelFieldComponent)
                {
                    if (HasCollisionWith(bitPixelFieldComponent, otherBitPixelFieldComponent))
                    {
                        collisionsFound = true;
                    }
                }
            }

            var collisionCheckComponent = entity.getComponent<CollisionCheckComponent>();
            collisionCheckComponent.HasCollisions = collisionsFound;
        }

        private bool HasCollisionWith(BitPixelFieldComponent first, BitPixelFieldComponent second)
        {
            var firstBounds = first.AnimatedSpriteBounds;
            var secondBounds = second.AnimatedSpriteBounds;

            int left = (int)Math.Max(firstBounds.Left, secondBounds.Left);
            int top = (int)Math.Max(firstBounds.Top, secondBounds.Top);
            int width = (int)Math.Min(firstBounds.Right, secondBounds.Right) - left;
            int height = (int)Math.Min(firstBounds.Bottom, secondBounds.Bottom) - top;

            var intersectingRectangle = new Rectangle(left, top, width, height);

            for (var x = intersectingRectangle.Left; x < intersectingRectangle.Right; x++)
            {
                for (var y = intersectingRectangle.Top; y < intersectingRectangle.Bottom; y++)
                {
                    var componentABit = first.PixelField[(x - (int)firstBounds.Left) + (y - (int)firstBounds.Top) * first.TextureWidth];
                    var componentBBit = second.PixelField[(x - (int)secondBounds.Left) + (y - (int)secondBounds.Top) * second.TextureWidth];

                    if (componentABit && componentBBit)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
