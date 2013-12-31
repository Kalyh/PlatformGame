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

        protected string _name;
        protected Bonus _bonusType = Bonus.None;
        protected float _bonusValue = 0f;
        #endregion

        #region ----- PROPERTIES -----
        public bool IsCollect { get { return _isCollect; } set { _isCollect = value; } }

        public string Name { get { return _name; } set { _name = value; } }
        public Bonus BonusType { get { return _bonusType; } set { _bonusType = value; } }
        public float BonusValue { get { return _bonusValue; } set { _bonusValue = value; } }
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
