using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBBA
{
    abstract class Entity
    {
        protected Point gridLocation
        {
            get
            {
                return new Point((int)(position.X / 32), (int)(position.Y / 32)); //FIX FOR ALL SIZES
            }
        }

        protected Rectangle NextCollisionBoxVertical { get { return new Rectangle((int)position.X, (int)(position.Y + movement.Y), width, height); } }
        protected Rectangle NextCollisionBoxHorizontal { get { return new Rectangle((int)(position.X + movement.X), (int)position.Y, width, height); } }
        protected Rectangle NextCollisionBox { get { return new Rectangle((int)(position.X + movement.X), (int)(position.Y + movement.Y), width, height); } }
        public Vector2 CenterPosition { get { return new Vector2(position.X + (width / 2), position.Y + (height / 2)); } }
        public Rectangle CollisionBox { get { return new Rectangle((int)position.X, (int)position.Y, width, height); } }

        protected Vector2 position;
        protected Vector2 movement;
        protected Texture2D spriteSheet;
        public Texture2D collisionSprite;
        protected Map map;
        protected int width;
        protected int height;
        public bool isActive;

        public Entity(Vector2 position, Texture2D spriteSheet, Map map, Texture2D collisionSprite)
        {
            this.collisionSprite = collisionSprite;
            this.position = position;
            this.spriteSheet = spriteSheet;
            this.map = map;
            width = collisionSprite.Width - 1; // Let's try to get this -1 out of here!
            height = collisionSprite.Height;
        }

        abstract protected void WorldCollision();
        abstract protected void EntityCollision();
        abstract public void Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch spriteBatch, Point cameraPosition);

        protected bool PixelCollision(Texture2D sprite1, Texture2D sprite2, Rectangle rect1, Rectangle rect2, float rotationRect1, float rotationRect2)
        {
            Color[] colorData1 = new Color[sprite1.Width * sprite1.Height];
            Color[] colorData2 = new Color[sprite2.Width * sprite2.Height];
            sprite1.GetData<Color>(colorData1);
            sprite2.GetData<Color>(colorData2);

            int top, bottom, left, right;
            top = Math.Max(rect1.Top, rect2.Top);
            bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            left = Math.Max(rect1.Left, rect2.Left);
            right = Math.Min(rect1.Right, rect2.Right);
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Vector2 v = Vector2.Transform(new Vector2(x, y), Matrix.CreateRotationZ(rotationRect1));
                    Color A = colorData1[(y - rect1.Top) * (rect1.Width) + (x - rect1.Left)];
                    //Color A = colorData1[(int)v.X + (int)v.Y];

                   // v = Vector2.Transform(new Vector2(x, y), Matrix.CreateRotationZ(rotationRect2));

                    //Color B = colorData2[((int)v.Y - rect2.Top) * (rect2.Width) + ((int)v.X - rect2.Left)];
                    Color B = colorData2[(y - rect2.Top) * (rect2.Width) + (x - rect2.Left)];
                    if (A.A != 0 && B.A != 0)
                        return true;
                }
            }
            return false;
        }
    }
}
