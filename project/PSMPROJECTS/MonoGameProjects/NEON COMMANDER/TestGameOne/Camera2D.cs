/// <author>
/// Aaron Cox
/// </author>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace TestGameOne
{
    /// <summary>
    /// Basic 2D camera class
    /// </summary>
    public class Camera2D
    {
        private Viewport viewport;

        public Camera2D(Viewport view)
        {
            viewport = view;
            Zoom = 1.0f;
            Rotation = 0.0f;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public float Rotation
        {
            get;
            set;
        }

        public float Zoom
        {
            get;
            set;
        }

        public Matrix Transform
        {
            get
            {
                return  Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateScale(Zoom) *
                        Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0.0f));
            }
        }


    }
}
