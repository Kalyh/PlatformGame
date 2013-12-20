using Gloopy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloopy.Model
{
    class Collectable : BaseSprite
    {
        #region ----- PROTECTED VARIABLE -----
        protected bool _isCollect = false;
        #endregion

        #region ----- PROPERTIES -----
        public bool IsCollect { get { return _isCollect; } set { _isCollect = value; } }
        #endregion

        #region ----- CONSTRUCTORS -----
        public Collectable(float x, float y, int width, int height, string textureName, MyGame game)
            : base(x, y, width, height, textureName, TypeSprite.Collectable, game)
        {
        }
        #endregion

        #region ----- PUBLIC METHODS -----
        #endregion
    }
}
