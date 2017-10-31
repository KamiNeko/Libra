using Nez;
using System;

namespace LibraCore.Components
{
    public class PeriodicVisibilityToggleComponent : Component
    {
        public string SoundEffectName { get; set; }
        public float TimeInvisible { get; set; }

        public DateTime LastActivationTime { get; set; }
    }
}
