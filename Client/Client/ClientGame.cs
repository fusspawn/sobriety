//#define LOCALHOST
#define EC2
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Client.Objects;
using Client.World;
using Client.Graphics;
using Client.UI;



namespace Client
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Network.NetworkClient Network;
        public static World.TerrainManager Terrain;
        public static ContentManager ContentManager;
        public static PlayerController Character;
        public static Camera2D Camera;
        public static GraphicsDevice GDevice;
        public static string Playername = "unknown";
        public static GameWindow GameWindow;
        


        public GameClient(string name)
        {
            Playername = name;
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            ContentManager = Content;
            GameWindow = this.Window;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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
            GDevice = GraphicsDevice;
#if LOCALHOST
            Network = new Network.NetworkClient("localhost", 9000);
#endif
#if EC2
            Network = new Network.NetworkClient("sobriety.dyndns.org", 9000);
#endif
            UIManager.Init(GDevice);
            Terrain = new World.TerrainManager();
            Character = new PlayerController();
            Camera = new Camera2D();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            InputManager.Update();
            UIManager.Update(gameTime);

            // TODO: Add your update logic here
            Network.Update();
            Character.Update(gameTime);
            ActorManager.Update(gameTime);
            Camera.Update();


            InputManager.PostUpdate();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Terrain.Draw(gameTime, spriteBatch);
            ActorManager.Draw(spriteBatch);
            UIManager.Draw();
            base.Draw(gameTime);
        }
    }
}
