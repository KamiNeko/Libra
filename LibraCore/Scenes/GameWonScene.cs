using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;

namespace LibraCore.Scenes
{
    public class GameWonScene : BaseScene
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

            var entity = CreateEntity("game-won-screen");
            var texture = ContentManager.Load<Texture2D>("game-won");
            var subtextures = Subtexture.subtexturesFromAtlas(texture, 640, 480);

            var animatedSprite = new Sprite<int>(subtextures[0]);
            animatedSprite.SetLocalOffset(new Vector2(320, 240));
            animatedSprite.AddAnimation(0, new SpriteAnimation(subtextures.GetRange(0, 3)) { fps = 15 });
            animatedSprite.Play(0);

            entity.addComponent(animatedSprite);
        }

        public override void Update()
        {
            base.Update();
            RaiseSkipEventOnKeyPress();
        }
    }
}
