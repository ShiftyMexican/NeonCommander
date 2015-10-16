using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestGameOne
{
    public struct Circle
    {
        public Vector2 m_center;
        public float m_radius;

        public Circle(Vector2 center, float radius)
        {
            m_center = center;
            m_radius = radius;
        }



        public bool Contains(Vector2 point)
        {
            Vector2 relativePosition = point - m_center;
            float distanceBetweenPoints = relativePosition.Length();

            if (distanceBetweenPoints <= m_radius) 
            {
                return true;
            }

            else 
            {
                return false; 
            }
        }

        public bool Intersects(Circle other)
        {
            Vector2 relativePosition = other.m_center - this.m_center;
            float distanceBetweenCenters = relativePosition.Length();
            if (distanceBetweenCenters <= this.m_radius + other.m_radius) { return true; }
            else { return false; }
        }
    }
}