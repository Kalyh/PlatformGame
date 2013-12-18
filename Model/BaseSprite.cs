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

        protected Rectangle _box;

        protected TypeSprite _typeSprite;

        protected MyGame _game;
        #endregion

        #region ----- PROPERTIES -----
        public string TextureName { get { return _textureName; } }
        public Texture2D Texture { get { return _texture; } }

        public Rectangle Box { get { return _box; } set { _box = value; } }

        public TypeSprite Type { get { return _typeSprite; } }
        #endregion

        #region ----- CONSTRUCTORS -----
        public BaseSprite(float x, float y, int width, int height, string textureName, TypeSprite type, MyGame game)
        {
            this._textureName = textureName;

            this._box = new Rectangle((int)x, (int)y, width, height);

            this._typeSprite = type;
            this._game = game;
        }
        #endregion

        #region ----- PUBLIC METHODS -----
        public void SetTexture(Texture2D texture)
        {
            this._texture = texture;
        }

        public void SetPositionX(int x)
        {
            _box.X = x;
        }

        public void SetPositionY(int y)
        {
            _box.Y = y;
        }
        #endregion
    }
}
