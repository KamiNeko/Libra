using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using System;

namespace LibraCore.Scenes
{
    public class GameOverScene : BaseScene
    {
        public EventHandler SceneSkipped;

        private void RaiseSkipEventOnKeyPress()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (Input.isKeyDown(Keys.Enter) || gamePadState.Buttons.X == ButtonState.Pressed)
            {
                SceneSkipped?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            
            var entity = CreateEntity("game-over-screen");

            var sprite = new Sprite(ContentManager.Load<Texture2D>("game-over"));
            sprite.SetLocalOffset(new Vector2(320, 240));
            entity.addComponent(sprite);
        }

        public override void Update()
        {
            base.Update();
            RaiseSkipEventOnKeyPress();
        }
    }
}
