using MyGame1.Utils;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame1.Model
{
    class Hero : Character
    {
        #region ---- PRIVATE VARIABLE ----
        private float _jump = 120;
        private float _currentJump = 0;

        private bool _canJump = true;
        private bool _isJumping = false;
        private bool _isFinishJump = true;
        #endregion

        #region ----- PROPERTIES -----
        public float Jump { get { return _jump; } }
        public float CurrentJump { get { return _currentJump; } set { _currentJump = value; } }

        public bool CanJump { get { return _canJump; } set { _canJump = value; } }
        public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
        public bool IsFinishJump { get { return _isFinishJump; } set { _isFinishJump = value; } }
        #endregion

        #region ----- CONSTRUCTORS -----
        public Hero(float x, float y, int width, int height, float speed, List<string> textureName, MyGame game)
            : base(x, y, width, height, textureName, game)
        {
            this._speed = new Vector2(speed, 0f);
        }
        #endregion

        #region ----- PUBLIC METHODS -----
        public void Movements(Keys direction)
        {
            switch (direction)
            {
                case Keys.Right:
                    this._position += this._speed;
                    if (this._position.X > this._game.WorldWidth)
                    {
                        this._position.X = this._game.WorldWidth;
                        break;
                    }

                    if (!this._isBlockedRight)
                    {
                        this._game.Camera.Move(this._speed);
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
                    this._position -= this._speed;
                    if (this._position.X < this._width)
                    {
                        this._position.X = this._width;
                        break;
                    }

                    if (!this._isBlockedLeft)
                    {
                        this._game.Camera.Move(- this._speed);
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
        #endregion
    }
}
