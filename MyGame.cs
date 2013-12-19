using MP3player;
using MyGame1.Model;
using MyGame1.Utils;
using SharpDX;
using SharpDX.IO;
using SharpDX.MediaFoundation;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MyGame1
{
    class MyGame : Game
    {
        #region ----- PRIVATE VARIABLE -----
        private GraphicsDeviceManager graphicsDeviceManager;
        private int _windowWidth = 864;
        private int _windowHeight = 480;

        private SpriteBatch spriteBatch;
        private int _sectionWidth = 32;
        private int _sectionHeight = 32;

        private Camera2D _camera;

        private Texture2D Background;
        private Hero hero;

        private string _worldPath = @"World\1.map";

        private int _worldWidth;
        private int _worldHeight;

        private List<BaseSprite> AllSprite = new List<BaseSprite>();
        private List<Character> Characters = new List<Character>();
        private List<Block> Blocks = new List<Block>();
        private List<Sprite> Sprites = new List<Sprite>();

        private SpriteFont arial16Font;

        private KeyboardManager keyboard;
        private KeyboardState keyboardState;

        private MouseManager mouse;
        private MouseState mouseState;
        
        private Stopwatch fpsTimer = new Stopwatch();
        private int fpsCounter = 0;

        private float _surface;
        private Vector2 _gravity = new Vector2(0, 9.8f);

        private AudioPlayer audiop;

        #endregion

        #region ----- PROPERTIES -----
        public float Surface { get { return _surface; } }
        public Vector2 Gravity { get { return _gravity; } }

        public Camera2D Camera { get { return _camera; } }

        public int WindowWidth { get { return _windowWidth; } }
        public int WindowHeight { get { return _windowHeight; } }

        public int WorldWidth { get { return _worldWidth; } }
        public int WorldHeight { get { return _worldHeight; } }
        #endregion

        #region ----- CONSTRUCTORS -----
        public MyGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredBackBufferHeight = _windowHeight;
            graphicsDeviceManager.PreferredBackBufferWidth = _windowWidth;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Initialize input keyboard system
            keyboard = new KeyboardManager(this);

            // Initialize input mouse system
            mouse = new MouseManager(this);

            audiop = new AudioPlayer("rocket");
        }
        #endregion

        #region ----- PROTECTED METHODS -----
        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "MyGame by Kalyh";
            fpsTimer.Start();

            base.Initialize();

            this.IsMouseVisible = true;
            _surface = this.Window.ClientBounds.Height;

            string[] lines = File.ReadAllLines(_worldPath);

            if (lines != null)
            {
                _worldHeight = lines.Count() * _sectionHeight;
                _worldWidth = lines[0].Split(' ').Count() * _sectionWidth;

                int x = 0;
                int y = 0;
                foreach (var line in lines)
                {
                    x = _sectionWidth;
                    string[] elements = line.Split(' ');
                    foreach (var element in elements)
                    {
                        switch (element)
                        {
                            case ".":
                                break;
                            case "S":
                                Blocks.Add(new Block(x, y, 32, 32, "Spike", this));
                                break;
                            case "H":
                                hero = new Hero(x, y, 32, 32, 5, new List<string>() { "Balls", "Balls45", "Balls90", "Balls135", "Balls180", "Balls225", "Balls270", "Balls315" }, this);
                                break;
                            default:
                                break;
                        }
                        x += _sectionWidth;
                    }
                    y += _sectionHeight;
                }
            }

            /*hero = new Hero(32f, _surface, 32, 32, 5, new List<string>() { "Balls", "Balls45", "Balls90", "Balls135", "Balls180", "Balls225", "Balls270", "Balls315" }, this);
            Blocks.Add(new Block(250f, _surface, 32, 32, "Spike", this));
            Blocks.Add(new Block(282f, _surface, 32, 32, "Spike", this));
            Blocks.Add(new Block(400f, _surface, 32, 32, "Spike", this));
            Blocks.Add(new Block(650f, _surface, 32, 32, "Spike", this));
            Blocks.Add(new Block(150f, _surface - 50, 32, 32, "Spike", this));*/


            Characters.Add(hero);

            AllSprite.AddRange(Characters);
            AllSprite.AddRange(Blocks);

            _camera = new Camera2D(this);

           audiop.Open(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\Content\03_rocket_flight.wav");
           audiop.Play();            
        }

        protected override void LoadContent()
        {
            // Instantiate a SpriteBatch
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            Background = Content.Load<Texture2D>("Background");

            foreach (var sprite in AllSprite)
            {
                sprite.SetTexture(Content.Load<Texture2D>(sprite.TextureName));
            }

            // Loads a sprite font
            // The [Arial16.xml] file is defined with the build action [ToolkitFont] in the project
            arial16Font = Content.Load<SpriteFont>("Arial16");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Get the current state of the keyboard
            keyboardState = keyboard.GetState();

            // Get the current state of the mouse
            mouseState = mouse.GetState();

            fpsCounter++;
            if (fpsTimer.ElapsedMilliseconds > 1000)
            {
                this.Window.Title = string.Format("MyGame by Kalyh - FPS: {0} ({1}ms)", Math.Round(1000.0 * fpsCounter / fpsTimer.ElapsedMilliseconds, 2), Math.Round((float)fpsTimer.ElapsedMilliseconds / fpsCounter, 2));
                fpsTimer.Reset();
                fpsTimer.Stop();
                fpsTimer.Start();
                fpsCounter = 0;
            }

            hero.DoJump((float)gameTime.ElapsedGameTime.TotalSeconds);

            var pressedKeys = keyboardState.GetPressedKeys();
            foreach (var key in pressedKeys)
            {
                if (key == Keys.Right || key == Keys.Left || key == Keys.Space)
                {
                    hero.Movements(key);
                }
                else if (key == Keys.Escape)
                {
                    this.Exit();
                }
                else
                {
                }
            }

            CheckForCollision();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.Black);

            // ------------------------------------------------------------------------
            // Use SpriteBatch to draw some balls on the screen using NonPremultiplied mode
            // as the sprite texture used is not premultiplied
            // ------------------------------------------------------------------------

            spriteBatch.Begin(SpriteSortMode.BackToFront, GraphicsDevice.BlendStates.AlphaBlend, null, null, null, null, _camera.get_transformation());
            spriteBatch.Draw(Background, new Rectangle(0, 0, _worldWidth, _worldHeight), Color.White);
            spriteBatch.End();

            foreach (BaseSprite item in AllSprite)
            {
                DrawSprite(item);
            }

            base.Draw(gameTime);
        }
        #endregion

        #region ----- PRIVATE METHODS -----
        private void DrawSprite(BaseSprite sprite)
        {
            Rectangle Pos;
            Vector2 Origin;
            switch (sprite.Type)
            {
                case TypeSprite.Character:
                    Pos = new Rectangle(0, 0, sprite.Box.Width, sprite.Box.Height);
                    Origin = new Vector2(sprite.Box.Width, sprite.Box.Height);
                    break;
                default:
                    Pos = new Rectangle(0, 0, sprite.Box.Width, sprite.Box.Height);
                    Origin = new Vector2(sprite.Box.Width, sprite.Box.Height);
                    break;
            }

            spriteBatch.Begin(SpriteSortMode.BackToFront, GraphicsDevice.BlendStates.NonPremultiplied, null, null, null, null, _camera.get_transformation()); //SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied
            spriteBatch.Draw(sprite.Texture, sprite.Box, Color.White);
            spriteBatch.End();
        }

        private void CheckForCollision()
        {
            if (hero.Box.Y < _surface)
            {
                hero.CanJump = false;
                hero.IsJumping = true;
            }

            foreach(var block in Blocks)
            {
                if (hero.Box.Intersects(block.Box))
                {
                    if (hero.Box.Bottom >= block.Box.Top && hero.Box.Bottom < (block.Box.Bottom - (block.Box.Height / 2)))
                    { //Collision bottom side of the hero
                        hero.SetPositionY(block.Box.Top - (hero.Box.Height));
                        hero.CanJump = true;
                        hero.IsJumping = false;
                        hero.Velocity = Vector2.Zero;
                    }
                    else if (hero.Box.Top <= block.Box.Bottom && hero.Box.Top > (block.Box.Top + (block.Box.Height / 2)))
                    {
                        hero.IsFinishJump = true;
                    }
                    else if (hero.Box.Left <= block.Box.Right && hero.Box.Left > (block.Box.Left + (block.Box.Width / 2)))
                    { //Collision left side of the hero
                        hero.SetPositionX(block.Box.Right);
                        hero.IsBlockedLeft = true;
                    }
                    else if (hero.Box.Right >= block.Box.Left && hero.Box.Right < (block.Box.Right - (block.Box.Width / 2)))
                    { //Collision right side of the hero
                        hero.SetPositionX(block.Box.Left - hero.Box.Width);
                        hero.IsBlockedRight = true;
                    }
                }
            }
        }
        #endregion
    }
}
