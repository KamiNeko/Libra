using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;

namespace LibraCore.Scenes
{
    public class TitleScreen : BaseScene
    {
        public EventHandler SceneSkipped;

        public override void Update()
        {
            base.Update();
            RaiseSkipEventOnKeyPress();
        }
        
        private void RaiseSkipEventOnKeyPress()
        {
            if (Input.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                SceneSkipped?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            var titelScreenEntity = BuildTitelScreenEntity();
            AddEntity(titelScreenEntity);
        }

        private Entity BuildTitelScreenEntity()
        {
            var entity = new Entity("title-screen");
            entity.addComponent(BuildTitleScreenSprite());
            return entity;
        }

        private Sprite BuildTitleScreenSprite()
        {
            var titleScreenTexture = ContentManager.Load<Texture2D>("title-screen");
            var titleScreenSubtextures = Subtexture.subtexturesFromAtlas(titleScreenTexture, 640, 480);

            var titleScreenSprite = new Sprite<int>(titleScreenSubtextures[0]);
            titleScreenSprite.SetLocalOffset(new Vector2(320, 240));

            var animation = new SpriteAnimation(titleScreenSubtextures.GetRange(0, 2)) { fps = 2f };
            titleScreenSprite.AddAnimation(0, animation);
            titleScreenSprite.Play(0);

            return titleScreenSprite;
        }
    }
}
