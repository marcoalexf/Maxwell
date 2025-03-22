using System.Collections.Generic;
using MainGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Player _player;
        private SpriteBatch _spriteBatch;

        private List<Bullet> bullets;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            bullets = new List<Bullet>();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a basic player
            _player = new Player(_spriteBatch, Content, this, new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2));
            _player.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime);

            foreach (var bullet in bullets)
            {
                bullet.Update(gameTime);
            }

            CheckBulletCollisions();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _player.Draw();

            foreach (var bullet in bullets)
            {
                bullet.Draw();
            }

            base.Draw(gameTime);
        }

        public void FireBullet(Vector2 position, SpriteEffects facingRight)
        {
            Vector2 bulletDirection = facingRight == SpriteEffects.None ? Vector2.UnitX : -Vector2.UnitX;

            Bullet newBullet = new Bullet(_spriteBatch, Content, position, bulletDirection, 500f);
            newBullet.LoadContent();

            bullets.Add(newBullet); // Add the new bullet to the list
        }

        private void CheckBulletCollisions()
        {
            foreach (var bullet in bullets)
            {
                // TODO: Physics i guess
            }
        }
    }
}
