using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BBBA
{
    class Projectile : Entity
    {
        Vector2 direction;

        float angle;
        float speed;
        float damage;
        float gravity;

        Character source;
        Texture2D gfx;
        
        public Projectile(Vector2 position, Texture2D sprite, Map map, Texture2D collisionSprite, Vector2 direction, float speed, Character source, float damage, float gravity) : base(position, sprite, map, collisionSprite)
        {
            this.damage = damage;
            this.source = source;
            isActive = true;
            this.speed = speed;
            this.direction = position + direction;
            this.gravity = gravity;
            gfx = sprite;
            movement = direction - position;
            angle = (float)Math.Atan2(movement.Y, movement.X);
            
            float movement_abs = (float)Math.Sqrt(Math.Pow(movement.X, 2) + (Math.Pow(movement.Y, 2)));
            movement.X = speed * movement.X / movement_abs;
            movement.Y = speed * movement.Y / movement_abs;
            position += movement;

            this.direction = movement;
        }

        //protected bool isNextCollideHorizontally(Rectangle a, Rectangle aNext, Rectangle b)
        //{

        //    if ((aNext.X + aNext.Width >= b.X && aNext.X <= b.X + b.Width) && (a.X + a.Width <= b.X || a.X >= b.X + b.Width))
        //        return true;
        //    return false;
        //}

        //protected bool isNextCollideVertically(Rectangle a, Rectangle aNext, Rectangle b)
        //{

        //    if ((aNext.Y + aNext.Height >= b.Y && aNext.Y <= b.Y + b.Height) && (a.Y + a.Height <= b.Y || a.Y >= b.Y + b.Height))
        //        return true;
        //    return false;
        //}

        protected void Movement(GameTime gameTime)
        {
            int elapsedMillis = gameTime.ElapsedGameTime.Milliseconds;
            movement = direction / elapsedMillis;
            movement.Y += gravity / elapsedMillis;
        }

        protected override void WorldCollision()
        {
            for (int i = gridLocation.X-3; i < gridLocation.X+3; i++)
            {
                for (int j = gridLocation.Y-3; j < gridLocation.Y+3; j++)
                {
                    if(map.Tiles[i, j].tileID != 0 && map.Tiles[i, j].tileID < 4)
                    {
                        if (CollisionBox.Intersects(map.Tiles[i, j].CollisionBox))
                        {
                            DeActivate();
                            Console.WriteLine(map.Tiles[i, j].CollisionBox.Left + " " + map.Tiles[i, j].CollisionBox.Right + " " + map.Tiles[i, j].CollisionBox.Top + " " + map.Tiles[i, j].CollisionBox.Bottom);
                            Console.WriteLine(CollisionBox.Left + " " + CollisionBox.Right + " " + CollisionBox.Top + " " + CollisionBox.Bottom);
                            Console.WriteLine();
                        }
                    }
                }
            }
        }

        protected override void EntityCollision()
        {
            for (int i = 0; i < map.Characters.Count; i++)
            {
                if(CollisionBox.Intersects(map.Characters[i].CollisionBox))
                {
                    if(PixelCollision(gfx, map.Characters[i].collisionSprite, CollisionBox, map.Characters[i].CollisionBox, angle, 0f) && map.Characters[i] != source)
                    {
                        DeActivate();
                        map.Characters[i].Hit(movement / 30, damage, source.friendly);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            WorldCollision();
            EntityCollision();
            Movement(gameTime);
            position += movement;
        }

        public override void Draw(SpriteBatch spriteBatch, Point cameraPosition)
        {
            spriteBatch.Draw(gfx, new Vector2(position.X - cameraPosition.X - (width / 2), position.Y - cameraPosition.Y - (height / 2)), null, Color.White, angle, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
        private void DeActivate()
        {
            isActive = false;
        }
    }
}