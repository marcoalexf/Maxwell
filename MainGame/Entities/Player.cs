using MainGame.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace MainGame.Entities;

public class Player(SpriteBatch spriteBatch, ContentManager contentManager, Vector2 initialPosition = default)
{

    // The reference to the AnimatedTexture for the character
    private AnimatedTexture spriteTexture = new AnimatedTexture(Vector2.Zero, rotation, scale, depth);
    // The rotation of the character on screen
    private const float rotation = 0;
    // The scale of the character, how big it is drawn
    private const float scale = 0.5f;
    // The draw order of the sprite
    private const float depth = 0.5f;

    // How many frames/images are included in the animation
    private const int frames = 8;
    // How many frames should be drawn each second, how fast does the animation run?
    private const int framesPerSec = 10;

    private Viewport viewport;

    protected Vector2 InputDirection = Vector2.Zero;

    public void LoadContent()
    {
        // "AnimatedCharacter" is the name of the sprite asset in the project.
        spriteTexture.Load(contentManager, "CharacterSheet", frames, framesPerSec);
    }

    public void Update(GameTime gameTime)
    {
        // Get keyboard state
        var keyboardState = Keyboard.GetState();

        InputDirection = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.W))
        {
            InputDirection.Y -= 1;
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            InputDirection.Y += 1;
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            InputDirection.X -= 1;
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            InputDirection.X += 1;
        }

        InputDirection.Normalize();

        // Assuming you have a position field to update
        //initialPosition += InputDirection * 20 * (float)gameTime.ElapsedGameTime.TotalSeconds;

        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        spriteTexture.UpdateFrame(elapsed);
    }

    public void Draw()
    {
        spriteBatch.Begin();
        Debug.WriteLine(initialPosition);
        spriteTexture.DrawFrame(spriteBatch, initialPosition);
        spriteBatch.End();
    }
}
