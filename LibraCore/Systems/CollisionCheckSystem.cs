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

                        // NOTE: Due to random order in the update method, we make sure to update the counterpart of the collision
                        //  so that we have consistency in collision detection, if we check the other component
                        if (otherBitPixelFieldComponent.Entity.getComponent<CollisionCheckComponent>() != null)
                        {
                            otherBitPixelFieldComponent.Entity.getComponent<CollisionCheckComponent>().HasCollisions = true;
                        }
                    }
                }
            }

            var collisionCheckComponent = entity.getComponent<CollisionCheckComponent>();
            collisionCheckComponent.HasCollisions = collisionsFound;
        }

        private bool HasCollisionWith(BitPixelFieldComponent first, BitPixelFieldComponent second)
        {
            if (first.PixelField == null || second.PixelField == null)
            {
                return false;
            }

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
