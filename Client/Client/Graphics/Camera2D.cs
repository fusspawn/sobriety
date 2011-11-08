using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SharedCode.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Graphics
{
    public class Camera2D
    {
        protected float          _zoom; // Camera Zoom
        public Matrix             _transform; // Matrix Transform
        public Vector2          _pos; // Camera Position
        protected float         _rotation; // Camera Rotation
 
        public Camera2D()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }

        // Sets and gets zoom
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public Matrix GetTransform(GraphicsDevice Device)
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(Device.Viewport.Width * 0.5f, Device.Viewport.Height * 0.5f, 0));
            return _transform;
        }

        
        public bool FollowObject = false;
        public Actor LookAtObject = null;

        public void Update() {
            if (FollowObject)
                _pos = LookAtObject.Location;

            if (InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.OemPlus))
                Zoom += 0.5f;

            if (InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.OemMinus))
                Zoom -= 0.5f;
        }

        public void LookAtActor(Actor Act, bool Follow) {
            LookAtObject = Act;
            FollowObject = Follow;
        }
    }
}
