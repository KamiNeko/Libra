using LibraCore.Components;
using Microsoft.Xna.Framework;
using Nez;
using System;

namespace LibraCore.Systems
{
    public class CollisionCheckSystem : EntityProcessingSystem
    {
        public CollisionCheckSystem() : base(new Matcher().all(
            typeof(BitPixelFieldComponent), 
            typeof(CollisionCheckComponent)))
        {
        }

        public override void process(Entity entity)
        {
            var collisionCheckComponent = entity.getComponent<CollisionCheckComponent>();
            var bitPixelFieldComponent = entity.getComponent<BitPixelFieldComponent>();

            var otherBitPixelFieldComponents = scene.FindComponentsOfType<BitPixelFieldComponent>();
            var collisionsFound = false;
            collisionCheckComponent.HasCollisionsWith.Clear();

            foreach (var otherBitPixelFieldComponent in otherBitPixelFieldComponents)
            {
                if (otherBitPixelFieldComponent != bitPixelFieldComponent)
                {
                    if (HasCollisionWith(bitPixelFieldComponent, otherBitPixelFieldComponent))
                    {
                        collisionsFound = true;

                        var otherEntity = otherBitPixelFieldComponent.Entity;
                        collisionCheckComponent.HasCollisionsWith.Add(otherEntity);
                    }
                }
            }
            
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
                    // NOTE: Sometimes one of the pixel fields is null
                    // TODO: Find out why we have a race condition here
                    if (first.PixelField == null || second.PixelField == null)
                    {
                        return false;
                    }

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
