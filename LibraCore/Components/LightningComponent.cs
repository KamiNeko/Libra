using Nez;
using System;

namespace LibraCore.Components
{
    public class LightningComponent : Component
    {
        public string SoundEffectName { get; set; }
        public float TimeInvisible { get; set; }

        public DateTime LastActivationTime { get; set; }
    }
}
