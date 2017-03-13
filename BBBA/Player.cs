using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BBBA
{

    class Player : Character
    {
        KeyboardState prevKbs;
        MouseState prevMs;


        public Player(Vector2 position, Texture2D spriteSheet, Map map, Texture2D collisionSprite,
            int maxHealth, float movementSpeed, float movementAcceleration,
            float friction, float gravity, float jumpSpeed, float maxFallSpeed, int lives)
            : base(position, spriteSheet, map, collisionSprite, maxHealth, movementSpeed, movementAcceleration, friction, gravity, jumpSpeed, maxFallSpeed, lives)
        {
            steadfast = true;
            friendly = true;
            runAnimationDelay = 50;
            runAnimationTimer = 0;
            lookDirection = 1;
        }

        public void Input(KeyboardState kbs, MouseState ms, Point cameraPosition)
        {
            moveLeft = false;
            moveRight = false;
            jump = false;
            isFiring = false;

            target = new Vector2(ms.X + cameraPosition.X, ms.Y + cameraPosition.Y);
            if (kbs.IsKeyDown(Keys.A))
            {
                moveLeft = true;
            }
            if (kbs.IsKeyDown(Keys.D))
            {
                moveRight = true;
            }
            if (kbs.IsKeyDown(Keys.Space))
            {
                jump = true;
            }

            if(kbs.IsKeyDown(Keys.Q) && !prevKbs.IsKeyDown(Keys.Q))
            {
                if(selectedWeapon == 0)
                {
                    selectedWeapon = weapons.Count-1;
                }
                else
                {
                    selectedWeapon--;
                }
            }

            if (kbs.IsKeyDown(Keys.E) && !prevKbs.IsKeyDown(Keys.E))
            {
                if (selectedWeapon >= weapons.Count - 1)
                {
                    selectedWeapon = 0;
                }
                else
                {
                    selectedWeapon++;
                }
            }

            if (ms.LeftButton == ButtonState.Pressed)
            {
                isFiring = true;
            }

            prevKbs = kbs;
            prevMs = ms;
        }
    }
}
