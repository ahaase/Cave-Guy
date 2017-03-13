using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBBA
{
    class Blagooparor : Character
    {
        int actionDelay;
        int actionDelayTimer;
        int action;
        Random r;

        public Blagooparor(Vector2 position, Texture2D spriteSheet, Map map, Texture2D collisionSprite,
            int maxHealth, float movementSpeed, float movementAcceleration,
            float friction, float gravity, float jumpSpeed, float maxFallSpeed, int lives)
            : base(position, spriteSheet, map, collisionSprite, maxHealth, movementSpeed, movementAcceleration, friction, gravity, jumpSpeed, maxFallSpeed, lives)
        {
            r = new Random((int)position.X * (int)position.Y);
            actionDelay = 150+(10 * (int)(r.NextDouble() * 100));
            actionDelayTimer = 0;
            runAnimationDelay = 350;
            runAnimationTimer = 0;
            lookDirection = 1;
            steadfast = true;
            friendly = false;
        }

        public void Input(GameTime gameTime, Vector2 target)
        {
            /*
            if(weaponCharge < weaponCooldown)
            {
                weaponCharge += gameTime.ElapsedGameTime.Milliseconds;
            }
            */
            this.target = target;
            isFiring = false;
            if (actionDelayTimer < actionDelay)
            {
                actionDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (actionDelayTimer >= actionDelay)
            {
                actionDelayTimer -= actionDelay;
                moveLeft = false;
                moveRight = false;
                jump = false;
                action = r.Next(0, 6);
            }
            switch (action)
            {
                case 0:
                    moveLeft = true;
                    break;
                case 1:
                    moveRight = true;
                    break;
                case 2:
                    jump = true;
                    break;
                case 3:
                    moveLeft = true;
                    jump = true;
                    break;
                case 4:
                    moveRight = true;
                    jump = true;
                    break;
                case 5:
                    isFiring = true;
                    break;
            }
        }
    }
}
