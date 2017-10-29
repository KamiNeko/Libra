using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace LibraCore.Components
{
    public class PlayerControllerComponent : Component, IUpdatable
    {
        public float Speed { get => speed; set => speed = value; }

        public void update()
        {
            
        }

        private float speed = 100f;
    }
}
