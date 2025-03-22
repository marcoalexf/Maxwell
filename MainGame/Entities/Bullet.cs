using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Input.InputListeners;

namespace MainGame.Entities;

enum BulletAnimations
{
    Fired,
    Traveling,
    Contact,
    Death
}

public class Bullet(SpriteBatch spriteBatch, ContentManager contentManager, Vector2 position, Vector2 direction, float speed)
{
    private SpriteSheet _spriteSheet;
    private AnimatedSprite _bulletAnimatedSprite;
    Vector2 initialPosition;

    public void LoadContent()
    {
        Texture2D bulletTexture = contentManager.Load<Texture2D>("projectile");
        Texture2DAtlas bullet = Texture2DAtlas.Create("Bullet/projectile", bulletTexture, 10, 10);
        _spriteSheet = new SpriteSheet("SpriteSheet/bullet-projectile", bullet);

        _spriteSheet.DefineAnimation(BulletAnimations.Fired.ToString(), builder =>
        {
            builder.IsLooping(false)
                   .AddFrame(regionIndex: 0, duration: TimeSpan.FromSeconds(0.1))
                   .AddFrame(1, TimeSpan.FromSeconds(0.1))
                   .AddFrame(2, TimeSpan.FromSeconds(0.1));
        });

        _spriteSheet.DefineAnimation(BulletAnimations.Traveling.ToString(), builder =>
        {
            builder.IsLooping(true)
                    .AddFrame(regionIndex: 3, duration: TimeSpan.FromSeconds(0.7))
                    .AddFrame(4, TimeSpan.FromSeconds(0.7));
        });

        _spriteSheet.DefineAnimation(BulletAnimations.Contact.ToString(), builder =>
        {
            builder.IsLooping(false)
                   .AddFrame(5, TimeSpan.FromSeconds(0.3))
                   .AddFrame(6, TimeSpan.FromSeconds(0.3));
        });

        _spriteSheet.DefineAnimation(BulletAnimations.Death.ToString(), builder =>
        {
            builder.IsLooping(false)
                   .AddFrame(7, TimeSpan.FromSeconds(0.9));
        });

        _bulletAnimatedSprite = new AnimatedSprite(_spriteSheet, BulletAnimations.Fired.ToString());
        _bulletAnimatedSprite.SetAnimation(BulletAnimations.Fired.ToString()).OnAnimationEvent += (sender, trigger) =>
        {
            if (trigger == AnimationEventTrigger.AnimationCompleted)
            {
                _bulletAnimatedSprite.SetAnimation(BulletAnimations.Traveling.ToString());
            }
        };

        initialPosition = position;
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        position += direction * speed * deltaTime;
        _bulletAnimatedSprite.Update(gameTime);

        //if (position != initialPosition) 
        //{
        //    _bulletAnimatedSprite.SetAnimation(BulletAnimations.Traveling.ToString());  
        //}
    }

    public void Draw()
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(_bulletAnimatedSprite, position, 0, new Vector2(10));
        spriteBatch.End();
    }
}
