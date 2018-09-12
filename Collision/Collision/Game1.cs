/* Bouncing ball on C#
 * Move the bath with Left and Right button
 * Press R to restart game and  and escape to pause it
 * Added random start after game is paused to prevent cheaters
 * Speed is increasing each time the ball touches the bat till it reaches a set speed
 * Added "breaking physics" feature to make the game harder
 * */
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Collision
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        public int counter = 0;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D ballTexture;
        Rectangle ballRectangle;

        //new object
        Texture2D batTexture;
        Rectangle batRectangle;


        Random myRandom = new Random();

        static Vector2 velocity;

        int screenWidth;
        int screenHeight;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            velocity.X = 3f;
            velocity.Y = 3f;

            RandomLoad();


            ballTexture = Content.Load<Texture2D>("1");
            ballRectangle = new Rectangle(300, 300, 40, 40);

            batTexture = Content.Load<Texture2D>("bat");
            batRectangle = new Rectangle(350, 550, 120, 20);

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
        }

        protected override void UnloadContent()
        {

        }
        public bool flag = false;
        protected override void Update(GameTime gameTime)
        {
            KeyboardState CurrentKeyboardState = Keyboard.GetState();

            //Bat Movement
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && batRectangle.X>=7)
                batRectangle.X = batRectangle.X - 7;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && batRectangle.X<=693)
                batRectangle.X = batRectangle.X + 7;

            //Escape and restart game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if(flag==false)
                {
                    flag = true;
                    velocity.X = 0;
                    velocity.Y = 0;
                }
                else
                {
                    flag = false;
                    RandomLoad();
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                LoadContent();
                RandomLoad();
                counter = 0;
            }


            //Bat and Ball Interact
            if(ballRectangle.Intersects(batRectangle))
            {
                velocity.Y = -velocity.Y;
                if (velocity.Y > -21f) { velocity.Y = velocity.Y - 3f; }
                counter++;
            }


            ballRectangle.X = ballRectangle.X + (int)velocity.X;
            ballRectangle.Y = ballRectangle.Y + (int)velocity.Y;


            if (ballRectangle.X <= 0)
                velocity.X = -velocity.X;

            if (ballRectangle.X + ballTexture.Width >= screenWidth)
            {velocity.X = -velocity.X;}

            if (ballRectangle.Y <= 0) {
                velocity.Y = -velocity.Y;
                //breaking physics so that game is not so easy
                if (counter % 6 == 0 && counter != 0)  { velocity.X = -velocity.X; }
                }

            if (ballRectangle.Y + ballTexture.Height >= screenHeight)
            {  MessageBox(new IntPtr(0), string.Format("Result: {0}", counter), "You died!", 0); this.Exit(); }


            base.Update(gameTime);
        }

        void RandomLoad()
        {
            //generate random head location
            int random = myRandom.Next(0, 4);
            if (random == 0)
            {
                velocity.X = 3f;
                velocity.Y = 3f;
            }
            if (random == 1)
            {
                velocity.X = -3f;
                velocity.Y = 3f;
            }
            if (random == 2)
            {
                velocity.X = -3f;
                velocity.Y = -3f;
            }
            if (random == 3)
            {
                velocity.X = 3f;
                velocity.Y = -3f;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(ballTexture, ballRectangle, Color.White);
            spriteBatch.Draw(batTexture, batRectangle, Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
