using MyGame1.Model;
using MyGame1.Utils;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private float _gravity = 5;
        #endregion

        #region ----- PROPERTIES -----
        public float Surface { get { return _surface; } }

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

                int x = _sectionWidth;
                int y = _sectionHeight;
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
                                hero = new Hero(x - 8, y - 8, 16, 16, 5, new List<string>() { "Balls", "Balls45", "Balls90", "Balls135", "Balls180", "Balls225", "Balls270", "Balls315" }, this);
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

            if(hero.IsJumping && !hero.IsFinishJump)
            {
                if (hero.CurrentJump >= hero.Jump)
                {
                    hero.IsFinishJump = true;
                }
                else
                {
                    hero.CurrentJump += _gravity;
                    hero.SetPosition(hero.Position.X, hero.Position.Y - _gravity);
                }
            }
            
            if (hero.IsJumping && hero.IsFinishJump && hero.Position.Y < _surface)
            {
                hero.CurrentJump -= _gravity;
                hero.SetPosition(hero.Position.X, hero.Position.Y + _gravity);
            }
            
            if(hero.Position.Y >= _surface)
            {
                hero.SetPosition(hero.Position.X, _surface);
                hero.IsJumping = false;
                hero.CanJump = true;
            }

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


            /*foreach (var pos in previousPos)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);
                spriteBatch.Draw(
                   ballsTexture,
                   new Vector2(pos[0], pos[1]),
                   new Rectangle(0, 0, 32, 32),
                   Color.White,
                   0.0f,
                   new Vector2(16, 16),
                   Vector2.One,
                   SpriteEffects.None,
                   0f);
                spriteBatch.End();
            }

            if (mouseState.Left == ButtonState.Pressed)
            {
                posX = mouseState.X * this.Window.ClientBounds.Right;
                posY = mouseState.Y * this.Window.ClientBounds.Bottom;
            }

            if (mouseState.Right == ButtonState.Pressed)
            {
                float tmpX = mouseState.X * this.Window.ClientBounds.Right;
                float tmpY = mouseState.Y * this.Window.ClientBounds.Bottom;

                spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);
                spriteBatch.Draw(
                   ballsTexture,
                   new Vector2(tmpX, tmpY),
                   new Rectangle(0, 0, 32, 32),
                   Color.White,
                   0.0f,
                   new Vector2(16, 16),
                   Vector2.One,
                   SpriteEffects.None,
                   0f);
                spriteBatch.End();

                previousPos.Add(new List<float> { tmpX, tmpY });
            }*/


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
            switch (sprite.Type)
            {
                case TypeSprite.Character:
                    Pos = new Rectangle(0, 0, sprite.Width, sprite.Height);
                    break;
                default:
                    Pos = new Rectangle(0, 0, sprite.Width, sprite.Height);
                    break;
            }

            spriteBatch.Begin(SpriteSortMode.BackToFront, GraphicsDevice.BlendStates.NonPremultiplied, null, null, null, null, _camera.get_transformation()); //SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied
            spriteBatch.Draw(
                sprite.Texture,
                sprite.Position,
                Pos,
                Color.White,
                0.0f,
                new Vector2(sprite.Width, sprite.Height),
                Vector2.One,
                SpriteEffects.None,
                0f);
            spriteBatch.End();
        }

        private void CheckForCollision()
        {
            BoundingBox boxHero = hero.GetBoundingBox();

            if (hero.Position.Y < _surface)
            {
                hero.CanJump = false;
                hero.IsJumping = true;
            }

            foreach(var block in Blocks)
            {
                BoundingBox boxBlock = block.GetBoundingBox();
                if (boxHero.Intersects(boxBlock))
                {
                    //Console.WriteLine("BEFORE Hero position: " + hero.Position);
                    //Console.WriteLine("BEFORE Hero min: " + boxHero.Minimum);
                    //Console.WriteLine("BEFORE Hero max: " + boxHero.Maximum);
                    //Console.WriteLine("BEFORE Block position: " + block.Position);
                    //Console.WriteLine("BEFORE Block min: " + boxBlock.Minimum);
                    //Console.WriteLine("BEFORE Block max: " + boxBlock.Maximum);
                    
                    //boxBlock = block.GetBoundingBox();
                    //boxHero = hero.GetBoundingBox();
                    //Console.WriteLine("AFTER Hero position: " + hero.Position);
                    //Console.WriteLine("AFTER Hero min: " + boxHero.Minimum);
                    //Console.WriteLine("AFTER Hero max: " + boxHero.Maximum);
                    //Console.WriteLine("AFTER Block position: " + block.Position);
                    //Console.WriteLine("AFTER Block min: " + boxBlock.Minimum);
                    //Console.WriteLine("AFTER Block max: " + boxBlock.Maximum);

                    if (boxHero.Maximum.Y >= boxBlock.Minimum.Y && boxHero.Maximum.Y < (boxBlock.Maximum.Y - (block.Height / 2)))
                    { //Collision bottom side of the hero
                        hero.SetPosition(hero.Position.X, block.Position.Y - block.Height);
                        hero.CanJump = true;
                        hero.IsJumping = false;
                    }
                    else if (boxHero.Minimum.Y <= boxBlock.Maximum.Y && boxHero.Minimum.Y > (boxBlock.Minimum.Y + (block.Height /2)))
                    {
                        hero.IsFinishJump = true;
                    }
                    else if (boxHero.Minimum.X <= boxBlock.Maximum.X && boxHero.Minimum.X > (boxBlock.Minimum.X + (block.Width / 2)))
                    { //Collision left side of the hero
                        hero.SetPosition(block.Position.X + (hero.Width), hero.Position.Y);
                        hero.IsBlockedLeft = true;
                    }
                    else if (boxHero.Maximum.X >= boxBlock.Minimum.X && boxHero.Maximum.X < (boxBlock.Maximum.X - (block.Width / 2)))
                    { //Collision right side of the hero
                        hero.SetPosition(block.Position.X - (hero.Width), hero.Position.Y);
                        hero.IsBlockedRight = true;
                    }
                }
            }
        }
        #endregion
    }
}
