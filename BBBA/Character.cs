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
    class Character : Entity
    {
        protected int lookDirection;
        protected int runAnimationTimer;
        protected int runAnimationDelay;

        protected int lives;
        protected int health;
        protected int maxHealth;
        protected int selectedWeapon;
        
        protected float movementSpeed;
        protected float movementAcceleration;
        protected float gravity;
        protected float jumpSpeed;
        protected float maxFallSpeed;
        protected float friction;
        protected float bulletSpeed;

        protected bool steadfast;
        protected bool moveLeft;
        protected bool moveRight;
        protected bool jump;
        protected bool isFiring;

        public bool friendly;

        protected List<Weapon> weapons;
        
        protected Vector2 spawnLocation;
        protected Vector2 target;

        public int Lives { get { return lives; } }
        public int Health { get { return health; } }

        public Character(Vector2 position, Texture2D spriteSheet, Map map, Texture2D collisionSprite,
            int maxHealth, float movementSpeed, float movementAcceleration,
            float friction, float gravity, float jumpSpeed, float maxFallSpeed, int lives) 
            : base(position, spriteSheet, map, collisionSprite)
        {
            this.maxHealth = maxHealth;
            health = maxHealth;
            this.movementSpeed = movementSpeed;
            this.movementAcceleration = movementAcceleration;
            this.friction = friction;
            this.gravity = gravity;
            this.jumpSpeed = jumpSpeed;
            this.maxFallSpeed = maxFallSpeed;
            this.lives = lives;
            
            isActive = true;
            spawnLocation = new Vector2(position.X, position.Y);
            selectedWeapon = 0;
            weapons = new List<Weapon>();
        }

        public override void Update(GameTime gameTime)
        {
            steadfast = movement.Y == 0;
            /*
            if (weaponCharge < weaponCooldown)
                weaponCharge += gameTime.ElapsedGameTime.Milliseconds;
                */
            Movement(gameTime);
            WorldCollision();
            EntityCollision();

            position += movement;

            if(weapons.Count > 0)
            {
                weapons[selectedWeapon].Update(gameTime, isFiring, CenterPosition, map, target, this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Point cameraPosition)
        {
            int spriteID = getSpriteID();
            if (lookDirection == 0)
            {
                spriteBatch.Draw(spriteSheet, new Vector2(position.X - cameraPosition.X, position.Y - cameraPosition.Y), new Rectangle(spriteID * width, 0, width, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
            }
            else {
                spriteBatch.Draw(spriteSheet, new Vector2(position.X - cameraPosition.X, position.Y - cameraPosition.Y), new Rectangle(spriteID * width, 0, width, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        public void Hit(Vector2 direction, float damage, bool otherFriendly)
        {
            if (otherFriendly != friendly)
            {
                health -= (int)damage;
                if (health <= 0)
                {
                    Die();
                }
            }
            movement += direction;
        }
        
        public void AddWeapon(Weapon weapon)
        {
            weapons.Add(weapon);
        }

        protected void Movement(GameTime gameTime)
        {
            int elapsedMillis = gameTime.ElapsedGameTime.Milliseconds;
            if (moveLeft && movement.X > -movementSpeed / elapsedMillis && !moveRight)
            {
                movement.X -= movementAcceleration / elapsedMillis;
                lookDirection = 0;
                runAnimationTimer += elapsedMillis;
            }
            else if (movement.X < 0)
            {
                if (movement.X > -friction / elapsedMillis)
                    movement.X = 0;
                else
                    movement.X += friction / elapsedMillis;
            }
            if (moveRight && movement.X < movementSpeed / elapsedMillis && !moveLeft)
            {
                movement.X += movementAcceleration / elapsedMillis;
                lookDirection = 1;
                runAnimationTimer += elapsedMillis;
            }
            else if (movement.X > 0)
            {
                if (movement.X < friction / elapsedMillis)
                    movement.X = 0;
                else
                    movement.X -= friction / elapsedMillis;
            }
            if (jump && steadfast)
            {
                movement.Y = -jumpSpeed / elapsedMillis;
            }
            else if (movement.Y < maxFallSpeed / elapsedMillis)
            {
                if (movement.Y + gravity > maxFallSpeed)
                    movement.Y = maxFallSpeed / elapsedMillis;
                else
                    movement.Y += gravity / elapsedMillis;
            }
        }

        protected void Die()
        {
            lives--;
            if (lives > 0)
                Respawn();
            else
                isActive = false;
        }

        protected void DeCollide(Rectangle otherCollisionBox, bool bounce)
        {
            // If the character needs to move horizontally
            while (NextCollisionBoxHorizontal.Intersects(otherCollisionBox) && movement.X != 0)
            {
                if (movement.X > 0)
                {
                    movement.X -= 0.1f;
                    if (movement.X < 1f)
                    {
                        movement.X = 0;
                    }
                }
                else if (movement.X < 0)
                {
                    movement.X += 0.1f;
                    if (movement.X > 1f)
                    {
                        movement.X = 0;
                    }
                }
            }

            // If the character needs to move vertically
            while (NextCollisionBoxVertical.Intersects(otherCollisionBox) && movement.Y != 0)
            {
                if (movement.Y > 0)
                {
                    movement.Y -= 0.1f;
                    if (movement.Y < 1f)
                    {
                        movement.Y = 0;
                        steadfast = true;
                    }
                }
                else if (movement.Y < 0)
                {
                    movement.Y += 0.1f;
                    if (movement.Y > 1f)
                    {
                        movement.Y = 0;
                    }
                }
            }

            // If the character needs to move both vertically and horizontally
            while (NextCollisionBox.Intersects(otherCollisionBox) && movement.Y != 0)
            {
                if (movement.Y > 0)
                {
                    movement.Y -= 0.1f;
                    if (movement.Y < 1f)
                    {
                        movement.Y = 0;
                        steadfast = true;
                    }
                }
                else if (movement.Y < 0)
                {
                    movement.Y += 0.1f;
                    if (movement.Y > 1f)
                    {
                        movement.Y = 0;
                    }
                }
                if (movement.X > 0)
                {
                    movement.X -= 0.1f;
                    if (movement.X < 1f)
                    {
                        movement.X = 0;
                    }
                }
                else if (movement.X < 0)
                {
                    movement.X += 0.1f;
                    if (movement.X > 1f)
                    {
                        movement.X = 0;
                    }
                }
            }
        }

        protected override void WorldCollision()
        {
            Vector2 bounceVector = new Vector2(movement.X / movement.X, movement.Y / movement.Y);
            for (int i = 0; i < map.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < map.Tiles.GetLength(1); j++)
                {
                    Rectangle otherCollisionBox = map.Tiles[i, j].CollisionBox;
                    if (NextCollisionBox.Intersects(map.Tiles[i, j].CollisionBox))
                    {

                        if (map.Tiles[i, j].tileID == 5)
                        {
                            jump = true;
                            movement.Y = -50f;
                        }
                        else if (map.Tiles[i, j].tileID != 0 && map.Tiles[i, j].tileID < 4)
                        {
                            DeCollide(otherCollisionBox, false);
                        }
                    }
                    if (NextCollisionBox.Intersects(map.Tiles[i, j].CollisionBox) && map.Tiles[i, j].tileID != 0 && map.Tiles[i, j].tileID < 4)
                    {
                        movement.Y = -bounceVector.Y;
                    }
                }
            }
        }

        protected override void EntityCollision()
        {
            for (int i = 0; i < map.Characters.Count; i++)
            {
                if (NextCollisionBox.Intersects(map.Characters[i].CollisionBox) && map.Characters[i] != this)
                {
                    DeCollide(map.Characters[i].CollisionBox, true);
                }
            }
        }

        private int getSpriteID()
        {
            int spriteID = 0;
            if (!moveLeft && !moveRight && !steadfast)
                spriteID = 0;
            else if (!steadfast)
                spriteID = 2;
            else if (moveLeft || moveRight)
            {
                if (runAnimationTimer > runAnimationDelay)
                {
                    runAnimationTimer = 0;
                    if (spriteID == 0)
                    {
                        spriteID = 1;
                    }
                    else if (spriteID == 1)
                    {
                        spriteID = 0;
                    }
                }
            }
            return spriteID;
        }

        private void Respawn()
        {
            movement = Vector2.Zero;
            health = maxHealth;
            position = spawnLocation;
        }
    }
}
