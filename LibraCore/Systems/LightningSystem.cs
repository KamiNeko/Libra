using LibraCore.Components;
using Microsoft.Xna.Framework.Audio;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraCore.Systems
{
    public class LightningSystem : EntityProcessingSystem
    {
        public LightningSystem() : base(new Matcher().all(typeof(LightningComponent), typeof(Sprite<int>)))
        {
            soundEffects = new Dictionary<string, SoundEffect>();
        }

        public override void onAdded(Entity entity)
        {
            base.onAdded(entity);

            var lighntingComponent = entity.getComponent<LightningComponent>();

            if (string.IsNullOrEmpty(lighntingComponent.SoundEffectName))
            {
                throw new InvalidOperationException($"The lightning component of entity {entity.name} must have a valid sound effect path");
            }

            if (!soundEffects.ContainsKey(lighntingComponent.SoundEffectName))
            {
                var soundEffect = scene.ContentManager.Load<SoundEffect>(lighntingComponent.SoundEffectName);
                soundEffects[lighntingComponent.SoundEffectName] = soundEffect;
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
            var lightningComponent = entity.getComponent<LightningComponent>();

            if (DateTime.Now - lightningComponent.LastActivationTime > TimeSpan.FromSeconds(lightningComponent.TimeInvisible))
            {
                var soundEffect = soundEffects[lightningComponent.SoundEffectName];
                soundEffect.CreateInstance().Play();

                var animationSpriteComponent = entity.getComponent<Sprite<int>>();
                animationSpriteComponent.Play(1);

                lightningComponent.LastActivationTime = DateTime.Now;
            }
        }

        private readonly IDictionary<string, SoundEffect> soundEffects;
    }
}
