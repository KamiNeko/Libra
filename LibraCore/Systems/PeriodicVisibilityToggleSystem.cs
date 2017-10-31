using LibraCore.Components;
using Microsoft.Xna.Framework.Audio;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;

namespace LibraCore.Systems
{
    public class PeriodicVisibilityToggleSystem : EntityProcessingSystem
    {
        public PeriodicVisibilityToggleSystem() : base(new Matcher().all(typeof(PeriodicVisibilityToggleComponent), typeof(Sprite<int>)))
        {
            soundEffects = new Dictionary<string, SoundEffect>();
        }

        public override void onAdded(Entity entity)
        {
            base.onAdded(entity);

            var periodicVisibilityToggleComponent = entity.getComponent<PeriodicVisibilityToggleComponent>();

#if DEBUG
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(periodicVisibilityToggleComponent.SoundEffectName), 
                $"The periodic visibility toggle component of entity {entity.name} must have a valid sound effect path");
#endif

            if (!soundEffects.ContainsKey(periodicVisibilityToggleComponent.SoundEffectName))
            {
                var soundEffect = scene.ContentManager.Load<SoundEffect>(periodicVisibilityToggleComponent.SoundEffectName);
                soundEffects[periodicVisibilityToggleComponent.SoundEffectName] = soundEffect;
            }

            var animationSpriteComponent = entity.getComponent<Sprite<int>>();
            animationSpriteComponent.OnAnimationCompletedEvent += (int key) =>
            {
                if (key == 1)
                {
                    animationSpriteComponent.Play(0);
                }
            };
        }

        public override void process(Entity entity)
        {
            var periodicVisibilityToggleComponent = entity.getComponent<PeriodicVisibilityToggleComponent>();

            if (DateTime.Now - periodicVisibilityToggleComponent.LastActivationTime > TimeSpan.FromSeconds(periodicVisibilityToggleComponent.TimeInvisible))
            {
                var soundEffect = soundEffects[periodicVisibilityToggleComponent.SoundEffectName];
                soundEffect.CreateInstance().Play();

                var animationSpriteComponent = entity.getComponent<Sprite<int>>();
                animationSpriteComponent.Play(1);

                periodicVisibilityToggleComponent.LastActivationTime = DateTime.Now;
            }
        }

        private readonly IDictionary<string, SoundEffect> soundEffects;
    }
}
