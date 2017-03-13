using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace BBBA
{
    class Map
    {
        public Tile[,] Tiles;
        Texture2D tileSpriteSheet;

        int mapWidth;
        int mapHeight;
        int tileSize;
        public List<Point> mobSpawns;
        public List<Character> Characters;
        public List<Projectile> Projectiles;
        public Point playerSpawn;
        public Texture2D tileCollisionTexture;

        public Map(int mapWidth, int mapHeight, Texture2D tileSpriteSheet, int tileSize, Texture2D tileCollisionTexture)
        {
            this.tileCollisionTexture = tileCollisionTexture;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.tileSpriteSheet = tileSpriteSheet;
            this.tileSize = tileSize;
            mobSpawns = new List<Point>();
            Projectiles = new List<Projectile>();

            Tiles = MapLoader(mapWidth, mapHeight);
        }

        public void ListCharacters(Character player, List<Character> mobs)
        {
            Characters = new List<Character>();
            Characters.Add(player);
            for(int i = 0; i < mobs.Count; i++)
            {
                Characters.Add(mobs[i]);
            }
        }

        private Tile[,] MapLoader(int mapWidth, int mapHeight)
        {
            string input = File.ReadAllText("content\\map.txt");

            int[,] map = new int[mapWidth, mapHeight];
            string[] lines = File.ReadAllLines("content\\map.txt");
            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    char ch = lines[y][x];
                    map[x, y] = ch-48;
                }
            }
            Tile[,] grid = new Tile[mapWidth, mapHeight]; // 100*40
            for(int x = 0; x < grid.GetLength(0); x++)
            {
                for(int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = new Tile(new Point(x, y), tileSpriteSheet, map[x, y], tileSize, tileCollisionTexture);
                    if(map[x, y] == 4)
                    {
                        playerSpawn = new Point(x, y);
                    }
                    else if(map[x, y] == 6)
                    {
                        mobSpawns.Add(new Point(x, y));
                    }
                }
            }
            return grid;
        }
    }
}
