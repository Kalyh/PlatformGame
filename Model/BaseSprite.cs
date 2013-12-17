using MyGame1.Utils;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame1.Model
{
    class BaseSprite
    {
        #region ----- PROTECTED VARIABLES -----
        protected string _textureName;
        protected Texture2D _texture;

        protected Vector2 _position = Vector2.Zero;
        protected Vector2 _speed = new Vector2(5f, 0f);

        protected TypeSprite _typeSprite;

        protected int _width;
        protected int _height;

        protected MyGame _game;
        #endregion

        #region ----- PROPERTIES -----
        public string TextureName { get { return _textureName; } }
        public Texture2D Texture { get { return _texture; } }

        public Vector2 Position { get { return _position; } }
        public Vector2 Speed { get { return _speed; } }

        public TypeSprite Type { get { return _typeSprite; } }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        #endregion

        #region ----- CONSTRUCTORS -----
        public BaseSprite(float x, float y, int width, int height, string textureName, TypeSprite type, MyGame game)
        {
            this._textureName = textureName;

            this._position = new Vector2(x, y);
            this._speed = new Vector2(10f, 0f);

            this._typeSprite = type;
            this._game = game;

            this._width = width;
            this._height = height;
        }
        #endregion

        #region ----- PUBLIC METHODS -----
        public void SetTexture(Texture2D texture)
        {
            this._texture = texture;
        }

        public void SetPosition(float x, float y)
        {
            _position = new Vector2(x, y);
        }

        public void SetSpeed(float speed)
        {
            this._speed = new Vector2(speed, 0);
        }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(new Vector3(_position.X - (_width / 2), _position.Y - (_height / 2), 0),
                                   new Vector3(_position.X + (_width / 2), _position.Y + (_height / 2), 0));
        }
        #endregion
    }
}
