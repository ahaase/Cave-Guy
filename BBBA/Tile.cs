using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBBA
{
    class Tile
    {
        Point gridLocation;
        Texture2D spriteSheet;
        public Texture2D collisionTex;
        public int tileID;
        int tileSize;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle((int)gridLocation.X * tileSize, (int)gridLocation.Y * tileSize, tileSize, tileSize);
            }
        }

        public Tile(Point gridLocation, Texture2D spriteSheet, int tileID, int tileSize, Texture2D collisionTex)
        {
            this.collisionTex = collisionTex;
            this.gridLocation = gridLocation;
            this.spriteSheet = spriteSheet;
            this.tileID = tileID;
            this.tileSize = tileSize;
        }
        public void Draw(SpriteBatch spriteBatch, Point cameraPosition)
        {
            if(tileID != 0)
                spriteBatch.Draw(spriteSheet, new Vector2((gridLocation.X * tileSize) - cameraPosition.X, (gridLocation.Y * tileSize) - cameraPosition.Y), new Rectangle((tileID % 4) * tileSize, (tileID / 4 * tileSize), tileSize, tileSize), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
