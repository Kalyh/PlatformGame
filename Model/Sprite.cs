﻿using MyGame1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame1.Model
{
    class Sprite : BaseSprite
    {
        #region ----- CONSTRUCTORS -----
        public Sprite(float x, float y, int width, int height, string textureName, MyGame game)
            : base(x, y, width, height, textureName, TypeSprite.Sprite, game)
        {
        }
        #endregion
    }
}
