using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace Idle_Civilization.Classes
{
    class Progressbar
    {
        public double time_elapsed;
        public double target_time;

        public bool running, finished, triggered;

        public Progressbar()
        {

        }

        public void Update(GameTime gameTime)
        {
            if(triggered && !running)
            {
                time_elapsed = target_time;
                running = true;
                triggered = false;
                finished = false;
            }

            if(running && !finished)
            {
                time_elapsed -= gameTime.ElapsedGameTime.TotalMilliseconds; 

                if(time_elapsed < 0)
                {
                    finished = true;
                    running = false;
                    triggered = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rectangle)
        {
            Rectangle background, frame, progress;

            background = new Rectangle();
            frame = new Rectangle();
            progress = new Rectangle();

            spriteBatch.Draw(Globals.primitive, background, Color.Black);
            spriteBatch.Draw(Globals.primitive, frame, Color.Yellow);
            spriteBatch.Draw(Globals.primitive, progress, Color.Yellow);
        }

    }
}
