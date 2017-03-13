using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BBBA
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int mapWidth, mapHeight, tileSize;
        int gameState;
        int selectionID;
        int difficulty;

        Map currentMap;
        Texture2D tileSpriteSheet;
        Texture2D playerSpriteSheet;
        Texture2D cursorTexture;
        Texture2D backgroundTexture;
        Texture2D blagooparorSpriteSheet;
        Texture2D playerCollisionSprite;
        Texture2D blagooparorCollisionSprite;
        Texture2D blagooparorProjectileTexture;
        Texture2D laserProjectileTexture;
        Texture2D tileCollisionTexture;
        Texture2D selectionArrow;
        Texture2D winScreen;
        Texture2D loseScreen;

        SpriteFont spriteFont;

        Player player;
        List<Blagooparor> blagooparors;
        Rectangle screenBounds;
        KeyboardState prevState;

        Point CameraPos
        {
            get
            {
                int x = (int)player.CenterPosition.X - screenBounds.Width / 2;
                if (x < 0) x = 0;
                else if (x + screenBounds.Width > mapWidth * tileSize) x = mapWidth * tileSize - screenBounds.Width;
                int y = (int)player.CenterPosition.Y - screenBounds.Height / 2;
                if (y < 0) y = 0;
                else if (y + screenBounds.Height > mapHeight * tileSize) y = mapHeight * tileSize - screenBounds.Height;

                return new Point(x, y);
            }
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            selectionID = 1;
            screenBounds = new Rectangle(0, 0, 900, 550);
            graphics.PreferredBackBufferWidth = screenBounds.Width;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = screenBounds.Height; ;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSpriteSheet = Content.Load<Texture2D>("PlayerSpriteSheet");
            tileSpriteSheet = Content.Load<Texture2D>("TileSpriteSheet");
            laserProjectileTexture = Content.Load<Texture2D>("laser");
            blagooparorProjectileTexture = Content.Load<Texture2D>("bullet");
            cursorTexture = Content.Load<Texture2D>("cursor");
            backgroundTexture = Content.Load<Texture2D>("background");
            blagooparorSpriteSheet = Content.Load<Texture2D>("BlagooparorSpriteSheet");
            playerCollisionSprite = Content.Load<Texture2D>("playerCollisionTex");
            blagooparorCollisionSprite = Content.Load<Texture2D>("blagooparorCollisionTex");
            tileCollisionTexture = Content.Load<Texture2D>("tileCollisionTex");
            selectionArrow = Content.Load<Texture2D>("SelectionArrow");
            winScreen = Content.Load<Texture2D>("WinScreen");
            loseScreen = Content.Load<Texture2D>("LoseScreen");

            spriteFont = Content.Load<SpriteFont>("SpriteFont");
            //Window.Position = new Point(0, 0);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void GameInitiation(int selection)
        {
            difficulty = selection;
            mapWidth = 100;
            mapHeight = 40;
            tileSize = 32;

            // Player Variables:

            int maxHealth = 110 - difficulty * 10;
            int weaponCooldown = 150 + difficulty * 20;
            float weaponDamage = 50 - difficulty * 10;
            float bulletSpeed = 500f;
            float bulletGravity = 0f;
            int weaponID = 0;
            float movementSpeed = 85f;
            float movementAcceleration = 2.5f;
            float friction = 4f;
            float gravity = 10f;
            float jumpSpeed = 145f;
            float maxFallSpeed = 250f;
            int lives = 4 - difficulty;

            // Blagooparor Variables:

            int maxHealthBlagooparor = 100;
            int weaponCooldownBlagooparor = 150;
            float weaponDamageBlagooparor = 10;
            float movementSpeedBlagooparor = 75f;
            float movementAccelerationBlagooparor = 2f;
            float frictionBlagooparor = 3f;
            float gravityBlagooparor = 10f;
            float jumpSpeedBlagooparor = 100f;
            float maxFallSpeedBlagooparor = 250f;
            float bulletSpeedBlagooparor = 100f;
            float bulletGravityBlagooparor = 0f;
            int weaponIDBlagooparor = 100;
            int livesBlagooparor = 1;

            currentMap = new Map(mapWidth, mapHeight, tileSpriteSheet, tileSize, tileCollisionTexture);
            int numberOfMobs = currentMap.mobSpawns.Count;

            player = new Player(new Vector2(currentMap.playerSpawn.X * tileSize, currentMap.playerSpawn.Y * tileSize), playerSpriteSheet, currentMap, playerCollisionSprite,
                maxHealth, movementSpeed, movementAcceleration, friction, gravity, jumpSpeed, maxFallSpeed, lives);
            player.AddWeapon(new Weapon(weaponDamage, bulletSpeed, bulletGravity, weaponCooldown, weaponID, laserProjectileTexture));
            player.AddWeapon(new Weapon(weaponDamageBlagooparor, bulletSpeedBlagooparor, bulletGravityBlagooparor, weaponCooldownBlagooparor, weaponIDBlagooparor, blagooparorProjectileTexture));

            blagooparors = new List<Blagooparor>();
            for (int i = 0; i < currentMap.mobSpawns.Count; i++)
            {
                blagooparors.Add(new Blagooparor(new Vector2(currentMap.mobSpawns[i].X * tileSize, currentMap.mobSpawns[i].Y * tileSize), blagooparorSpriteSheet, currentMap, blagooparorCollisionSprite,
                    maxHealthBlagooparor, movementSpeedBlagooparor, movementAccelerationBlagooparor, frictionBlagooparor, gravityBlagooparor, jumpSpeedBlagooparor, maxFallSpeedBlagooparor, livesBlagooparor));
                //blagooparors[i].AddWeapon(new Weapon(weaponDamageBlagooparor, bulletSpeedBlagooparor, bulletGravityBlagooparor, weaponCooldownBlagooparor, weaponIDBlagooparor, blagooparorProjectileTexture));
            }
            currentMap.ListCharacters(player, new List<Character>(blagooparors));
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch(gameState) // 0 = start, 1  = ingame, 2 = pause, 3 = win, 4 = loss
            {
                case 0:
                    if(((Keyboard.GetState().IsKeyDown(Keys.W) && !prevState.IsKeyDown(Keys.W)) || (Keyboard.GetState().IsKeyDown(Keys.Up) && !prevState.IsKeyDown(Keys.Up))) && selectionID > 0)
                    {
                        selectionID--;
                    }
                    else if (((Keyboard.GetState().IsKeyDown(Keys.S) && !prevState.IsKeyDown(Keys.S)) || (Keyboard.GetState().IsKeyDown(Keys.Down) && !prevState.IsKeyDown(Keys.Down))) && selectionID < 2)
                    {
                        selectionID++;
                    }
                    else if((Keyboard.GetState().IsKeyDown(Keys.Space) && !prevState.IsKeyDown(Keys.Space)) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = 1;
                        GameInitiation(selectionID);
                    }
                        break;
                case 1:
                    player.Input(Keyboard.GetState(), Mouse.GetState(), CameraPos);
                    player.Update(gameTime);

                    for (int i = 0; i < currentMap.Projectiles.Count; i++)
                    {
                        currentMap.Projectiles[i].Update(gameTime);
                        if (!currentMap.Projectiles[i].isActive)
                        {
                            currentMap.Projectiles.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < blagooparors.Count; i++)
                    {
                        if (blagooparors[i].isActive)
                        {
                            blagooparors[i].Input(gameTime, player.CenterPosition);
                            blagooparors[i].Update(gameTime);
                        }
                        else {
                            blagooparors.RemoveAt(i);
                            currentMap.Characters.RemoveAt(i + 1);
                            if (blagooparors.Count == 0)
                            {
                                gameState = 3;
                            }
                        }
                    }
                    if(player.Lives <= 0)
                    {
                        gameState = 4;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevState.IsKeyDown(Keys.Escape))
                    {
                        gameState = 2;
                    }
                    break;
                case 2:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevState.IsKeyDown(Keys.Escape))
                    {
                        gameState = 1;
                    }
                    break;
                case 3:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = 0;
                        GameInitiation(selectionID);
                    }
                    break;
                case 4:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = 0;
                        GameInitiation(selectionID);
                    }
                    break;
            }
            prevState = Keyboard.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SeaGreen);

            spriteBatch.Begin();

            switch (gameState) // 0 = start, 1  = ingame, 2 = pause, 3 = win, 4 = loss
            {
                case 0:
                    DrawMenu(spriteBatch);
                    break;
                case 1:
                    DrawGame(spriteBatch);
                    break;
                case 2:
                    DrawGame(spriteBatch);
                    spriteBatch.DrawString(spriteFont, "Game Paused\nEscape to resume...", new Vector2(100, 200), Color.Red);
                    break;
                case 3:
                    spriteBatch.Draw(winScreen, new Rectangle(0, 0, screenBounds.Width, screenBounds.Height), Color.White);
                    break;
                case 4:
                    spriteBatch.Draw(loseScreen, new Rectangle(0, 0, screenBounds.Width, screenBounds.Height), Color.White);
                    break;
            }

            spriteBatch.Draw(cursorTexture, new Vector2(Mouse.GetState().X - (cursorTexture.Width / 2), Mouse.GetState().Y - (cursorTexture.Height / 2)), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, "Health: " + player.Health, Vector2.Zero, Color.Red);
            spriteBatch.DrawString(spriteFont, "Lives: " + player.Lives, new Vector2(0, 50), Color.Red);
            spriteBatch.DrawString(spriteFont, "Enemies remaining: " + (currentMap.Characters.Count - 1), new Vector2(screenBounds.Width, screenBounds.Height) - spriteFont.MeasureString("Enemies remaining: " + (currentMap.Characters.Count - 1)), Color.Red);
        }

        private void DrawMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(selectionArrow, new Vector2(70, 207 + (40 * selectionID)), Color.White);
            spriteBatch.DrawString(spriteFont, "Easy", new Vector2(100, 200), Color.White);
            spriteBatch.DrawString(spriteFont, "Normal", new Vector2(100, 240), Color.Silver);
            spriteBatch.DrawString(spriteFont, "Hard", new Vector2(100, 280), Color.Black);
            spriteBatch.DrawString(spriteFont, "Up/Down to navigate", new Vector2(100, 400), Color.White);
            spriteBatch.DrawString(spriteFont, "Spacebar to start", new Vector2(100, 440), Color.White);
            spriteBatch.DrawString(spriteFont, "Game Objective:", new Vector2(350, 40), Color.Black);
            spriteBatch.DrawString(spriteFont, "Eliminate the...", new Vector2(350, 80), Color.Black);
            spriteBatch.DrawString(spriteFont, "... Blagooparors!", new Vector2(420, 120), Color.Red);
            spriteBatch.DrawString(spriteFont, "Game Controls:", new Vector2(350, 220), Color.Black);
            spriteBatch.DrawString(spriteFont, "Mouse1 to fire", new Vector2(600, 220), Color.Black);
            spriteBatch.DrawString(spriteFont, "A to move left", new Vector2(600, 260), Color.Black);
            spriteBatch.DrawString(spriteFont, "D to move right", new Vector2(600, 300), Color.Black);
            spriteBatch.DrawString(spriteFont, "Spacebar to jump", new Vector2(600, 340), Color.Black);
            spriteBatch.DrawString(spriteFont, "Q/E to cycle weapons", new Vector2(580, 380), Color.Black);
        }

        private void DrawGame(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Rectangle(-CameraPos.X / 10, -CameraPos.Y / 10, backgroundTexture.Width, backgroundTexture.Height), Color.White);

            DrawProjectiles(spriteBatch);
            DrawCharacters(spriteBatch);
            DrawMap(spriteBatch);
            DrawHud(spriteBatch);
        }

        private void DrawCharacters(SpriteBatch spriteBatch)
        {
            foreach (Character character in currentMap.Characters)
            {
                character.Draw(spriteBatch, CameraPos);
            }
        }

        private void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile projectile in currentMap.Projectiles)
            {
                projectile.Draw(spriteBatch, CameraPos);
            }
        }

        private void DrawMap(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < currentMap.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < currentMap.Tiles.GetLength(1); j++)
                {
                    currentMap.Tiles[i, j].Draw(spriteBatch, CameraPos);
                }
            }
        }
    }
}
