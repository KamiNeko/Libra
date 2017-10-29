using LibraCore.Components;
using Microsoft.Xna.Framework;
using Nez;
using System.Linq;

namespace LibraCore.Systems
{
    public class ScriptedMovementSystem : EntityProcessingSystem
    {
        public ScriptedMovementSystem() : base(new Matcher().all(typeof(ScriptedMovementComponent)))
        {
        }

        public void Reset()
        {
            var iterationSafeEntitesCopy = _entities.ToArray();

            foreach (var entity in iterationSafeEntitesCopy)
            {
                var scriptedMovementComponent = entity.getComponent<ScriptedMovementComponent>();

                if (scriptedMovementComponent.Paths.Count() > 0)
                {
                    scriptedMovementComponent.CurrentPathIndex = 0;
                    entity.position = scriptedMovementComponent.Paths[0];
                }
            }
        }

        public override void process(Entity entity)
        {
            var scriptedMovementComponent = entity.getComponent<ScriptedMovementComponent>();
            var nextTarget = GetNextPathPoint(scriptedMovementComponent);

            var direction = nextTarget - entity.position;
            var remainingLength = direction.Length();
            var normalizedDirection = direction;

            if (remainingLength != 0.0f)
            {
                normalizedDirection = direction / remainingLength;
            }

            var nextStepSizeInPixels = scriptedMovementComponent.SpeedInPixelsPerSecond * Time.deltaTime;

            if (nextStepSizeInPixels > remainingLength)
            {
                nextStepSizeInPixels = remainingLength;
                scriptedMovementComponent.CurrentPathIndex++;
            }

            entity.position += normalizedDirection * nextStepSizeInPixels;
        }

        private Vector2 GetNextPathPoint(ScriptedMovementComponent scriptedMovementComponent)
        {
            var pathsCount = scriptedMovementComponent.Paths.Count();

            if (scriptedMovementComponent.CurrentPathIndex >= pathsCount)
            {
                scriptedMovementComponent.CurrentPathIndex = 0;
            }

            return scriptedMovementComponent.Paths[scriptedMovementComponent.CurrentPathIndex];
        }
    }
}
