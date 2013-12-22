using MP3player;
using Gloopy.Utils;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloopy.Model
{
    class Hero : Character
    {
        #region ---- PRIVATE VARIABLE ----
        private float _jump = 120;
        private float _currentJump = 0;

        private bool _canJump = true;
        private bool _isJumping = false;
        private bool _isFinishJump = true;

        private Vector2 _velocity = Vector2.Zero;

        private List<Collectable> _bonus = new List<Collectable>();

        private AudioPlayer audiop;
        #endregion

        #region ----- PROPERTIES -----
        public float Jump { get { return _jump; } }
        public float CurrentJump { get { return _currentJump; } set { _currentJump = value; } }

        public bool CanJump { get { return _canJump; } set { _canJump = value; } }
        public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
        public bool IsFinishJump { get { return _isFinishJump; } set { _isFinishJump = value; } }

        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

        public List<Collectable> Bonus { get { return _bonus; } }
        #endregion

        #region ----- CONSTRUCTORS -----
        public Hero(float x, float y, int width, int height, float speed, List<string> textureName, MyGame game)
            : base(x, y, width, height, textureName, game)
        {
            this._speed = speed;
            audiop = new AudioPlayer("jump");
          //  audiop.Open(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\Content\Mario_saut.wav");
        }
        #endregion

        #region ----- PUBLIC METHODS -----
        public void Movements(Keys direction)
        {
            switch (direction)
            {
                case Keys.Right:
                    this._box.X += (int)this._speed;

                    if (this._box.Right > this._game.WorldWidth)
                    {
                        this._box.X = this._game.WorldWidth - this._box.Width;
                        break;
                    }

                    if (!this._isBlockedRight)
                    {
                        if (this._box.X >= this._game.Camera.Pos.X)
                        {
                            this._game.Camera.Move(new Vector2(this._speed, 0));
                        }

                        if (this._currentTextureIndex + 1 <= (this.ListTexture.Count() - 1))
                        {
                            this._currentTextureIndex += 1;
                        }
                        else
                        {
                            this._currentTextureIndex = 0;
                        }

                        _texture = _game.Content.Load<Texture2D>(this.ListTexture[this.CurrentTextureIndex]);
                    }
                    break;
                case Keys.Left:
                    this._box.X -= (int)this._speed;

                    if (this._box.Left < 0)
                    {
                        this._box.X = 0;
                        break;
                    }

                    if (!this._isBlockedLeft)
                    {
                        if (this._box.X <= this._game.Camera.Pos.X)
                        {
                            this._game.Camera.Move(new Vector2(-this._speed, 0));
                        }

                        if (this._currentTextureIndex - 1 >= 0)
                        {
                            this._currentTextureIndex -= 1;
                        }
                        else
                        {
                            this._currentTextureIndex = (this.ListTexture.Count() - 1);
                        }

                        _texture = _game.Content.Load<Texture2D>(this.ListTexture[this.CurrentTextureIndex]);
                    }
                    break;
                case Keys.Space:
                    if (_canJump)
                    {
                        audiop.Open(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\Content\Mario_saut.wav");
                        audiop.Play();

                        _currentJump = 0;
                        _canJump = false;
                        _isJumping = true;
                        _isFinishJump = false;
                    }
                    break;
            }
            this._isBlockedLeft = false;
            this._isBlockedRight = false;
        }

        public void DoJump(float time)
        {
            if (!_isJumping)
            {
                audiop.kill();
            }
            
            if (_isJumping && !_isFinishJump)
            {
                _velocity += this._game.Gravity * time;

                _box.Y -= (int)(this._game.Gravity - _velocity).Y;

                Console.WriteLine("Box Y: " + _box.Y + ", Velocity: " + _velocity.Y + ", Box calcul: " + (this._game.Gravity - _velocity).Y);
            }
            
            if (_isJumping && _isFinishJump && _box.Y < this._game.Surface)
            {
                //Améliorer la chute
                /*_velocity += this._game.Gravity * time;

                _box.Y -= (int)(this._game.Gravity - _velocity).Y;
                Console.WriteLine("Box Y: " + _box.Y + ", Velocity: " + _velocity.Y + ", Box calcul: " + (this._game.Gravity - _velocity).Y);*/

                _box.Y += (int)this._game.Gravity.Y;
            }

            if (_box.Bottom >= this._game.Surface)
            {
                _box.Y = (int)this._game.Surface - _box.Height;
                _velocity = Vector2.Zero;
                _isJumping = false;
                _isFinishJump = true;
                _canJump = true;
            }
        }

        public void AddBonus(Collectable bonus)
        {
            _bonus.Add(bonus);
        }
        #endregion
    }
}
