using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame1.Model
{
    class Camera2D
    {
        private Matrix _transform; // Matrix Transform
        private Vector2 _pos; // Camera Position
        private MyGame _game;

        // Get set position
        public Matrix Transform { get { return _transform; } set { _transform = value; } }
        public Vector2 Pos { get { return _pos; } set { _pos = value; } }
 
        public Camera2D(MyGame game)
        {
            _game = game;
            _pos = new Vector2(_game.WindowWidth / 2, _game.WindowHeight / 2);
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            Vector2 tmp = _pos + amount;
            if((tmp.X - (_game.WindowWidth / 2)) <= 0)
            {
                return;
            }
            else if ((tmp.X + (_game.WindowWidth / 2)) >= _game.WorldWidth)
            {
                return;
            }

            _pos += amount;
        }

        public Matrix get_transformation()
        {
            _transform = Matrix.Translation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.Translation(new Vector3(_game.WindowWidth * 0.5f, _game.WindowHeight * 0.5f, 0));
            return _transform;
        }
    }
}
