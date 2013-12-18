using MyGame1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame1.Model
{
    class Character : BaseSprite
    {
        #region ----- PROTECTED VARIABLE -----
        protected float _speed = 5f;

        protected string[] _listTexture;

        protected int _currentTextureIndex = 0;

        protected bool _isBlockedLeft = false;
        protected bool _isBlockedRight = false;
        #endregion

        #region ----- PROPERTIES -----
        public float Speed { get { return _speed; } set { _speed = value;} }

        public string[] ListTexture { get { return _listTexture; } }

        public int CurrentTextureIndex { get { return _currentTextureIndex; } }

        public bool IsBlockedLeft { get { return _isBlockedLeft; } set { _isBlockedLeft = value; } }
        public bool IsBlockedRight { get { return _isBlockedRight; } set { _isBlockedRight = value; } }
        #endregion

        #region ----- CONSTRUCTORS -----
        public Character(float x, float y, int width, int height, List<string> textureName, MyGame game)
            : base(x, y, width, height, textureName[0], TypeSprite.Character, game)
        {
            _listTexture = textureName.ToArray();
        }
        #endregion

        #region ----- PUBLIC METHOD -----
        #endregion
    }
}
