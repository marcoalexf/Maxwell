using MainGame.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Diagnostics;

namespace MainGame.Entities;

enum PlayerAnimations
{
    Idle,
    Attack,
    Move
}

public class Player(SpriteBatch spriteBatch, ContentManager contentManager, Game1 gameInstance, Vector2 position = default)
{
    private SpriteSheet _spriteSheet;
    private AnimatedSprite _playerAnimatedSprite;

    private KeyboardListener _keyboardListener;

    SpriteEffects facingRight = SpriteEffects.None;

    private readonly float fireCooldown = 0.5f;
    private float timeSinceLastShot = 0f;


    public void LoadContent()
    {
        Texture2D playerTexture = contentManager.Load<Texture2D>("player");
        Texture2DAtlas player = Texture2DAtlas.Create("Player/player", playerTexture, 50, 37);
        _spriteSheet = new SpriteSheet("SpriteSheet/player", player);

        _spriteSheet.DefineAnimation(PlayerAnimations.Attack.ToString(), builder =>
        {
            builder.IsLooping(true)
                   .AddFrame(regionIndex: 0, duration: TimeSpan.FromSeconds(0.1))
                   .AddFrame(1, TimeSpan.FromSeconds(0.1))
                   .AddFrame(2, TimeSpan.FromSeconds(0.1))
                   .AddFrame(3, TimeSpan.FromSeconds(0.1))
                   .AddFrame(4, TimeSpan.FromSeconds(0.1))
                   .AddFrame(5, TimeSpan.FromSeconds(0.1));
        });

        _spriteSheet.DefineAnimation(PlayerAnimations.Idle.ToString(), builder =>
        {
            builder.IsLooping(true)
                    .AddFrame(regionIndex: 6, duration: TimeSpan.FromSeconds(0.1))
                    .AddFrame(7, TimeSpan.FromSeconds(0.1))
                    .AddFrame(8, TimeSpan.FromSeconds(0.1))
                    .AddFrame(9, TimeSpan.FromSeconds(0.1));
        });

        _spriteSheet.DefineAnimation(PlayerAnimations.Move.ToString(), builder =>
        {
            builder.IsLooping(true)
                   .AddFrame(10, TimeSpan.FromSeconds(0.1))
                   .AddFrame(11, TimeSpan.FromSeconds(0.1))
                   .AddFrame(12, TimeSpan.FromSeconds(0.1))
                   .AddFrame(13, TimeSpan.FromSeconds(0.1))
                   .AddFrame(14, TimeSpan.FromSeconds(0.1))
                   .AddFrame(15, TimeSpan.FromSeconds(0.1));
        });

        _playerAnimatedSprite = new AnimatedSprite(_spriteSheet, PlayerAnimations.Idle.ToString());

        _keyboardListener = new KeyboardListener();
        _keyboardListener.KeyPressed += (sender, eventArgs) =>
        {
            Debug.WriteLine($"Current pressed key: {eventArgs.Key}");
            if (WASDKeysPressed(eventArgs.Key))
            {
                _playerAnimatedSprite.SetAnimation(PlayerAnimations.Move.ToString());
            } else if (eventArgs.Key == Keys.Enter) 
            {
                _playerAnimatedSprite.SetAnimation(PlayerAnimations.Attack.ToString());
            }
            else
            {
                _playerAnimatedSprite.SetAnimation(PlayerAnimations.Idle.ToString());
            }
        };
    }

    public void Update(GameTime gameTime)
    {
        // Get keyboard state
        var state = Keyboard.GetState();
        float speed = 200f; // Adjust speed as needed
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _keyboardListener.Update(gameTime);

        if (state.IsKeyDown(Keys.W))
            position.Y -= speed * deltaTime;
        if (state.IsKeyDown(Keys.S))
            position.Y += speed * deltaTime;
        if (state.IsKeyDown(Keys.A))
        { 
            position.X -= speed * deltaTime;
            facingRight = SpriteEffects.FlipHorizontally;
        }
        if (state.IsKeyDown(Keys.D))
        {
            facingRight = SpriteEffects.None;
            position.X += speed * deltaTime;
        }

        // ✅ Update cooldown timer
        if (timeSinceLastShot > 0)
        {
            timeSinceLastShot -= deltaTime;
        }

        // ✅ Fire a bullet if SPACE is pressed and cooldown is ready
        if (state.IsKeyDown(Keys.Space) && timeSinceLastShot <= 0)
        {
            FireBullet();
            timeSinceLastShot = fireCooldown; // Reset cooldown
        }

        _playerAnimatedSprite.Update(gameTime);
    }

    public void Draw()
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        int scale = 3;
        _playerAnimatedSprite.Effect = facingRight;
        spriteBatch.Draw(_playerAnimatedSprite, position, 0, new Vector2(scale));
        spriteBatch.End();
    }

    private void FireBullet()
    {
        gameInstance?.FireBullet(position, facingRight);
    }

    private bool WASDKeysPressed(Keys key) => key == Keys.W || key == Keys.S || key == Keys.A || key == Keys.D;
}
