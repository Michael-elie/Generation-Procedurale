using System.Collections.Generic;
using UnityEngine;



public class Vertex // sommets
{
    private float _x;
    private float _y;

    public Vertex(float x, float y)
    {
        _x = x;
        _y = y;
    }

    public bool Equals(Vertex otherVertex)
    {
        // return (_x == otherVertex._x && _y == otherVertex._y);
        return (Mathf.Approximately(_x, otherVertex._x) && Mathf.Approximately(_y, otherVertex._y));
    }


    public class Edge // aretes
    {
        private Vector2 _vertex0, _vertex1;
        public Edge(Vector2 vertex0, Vector2 vertex1) {
            _vertex0 = vertex0;
            _vertex1 = vertex1;
        }
        public bool Equals(Edge otherEdge) {
            return (_vertex0 == otherEdge._vertex0 && _vertex1 == otherEdge._vertex1) ||
                   (_vertex0 == otherEdge._vertex1 && _vertex1 == otherEdge._vertex0);
        }
    }

    public class Triangle
    {
        private Vector2 _vertex0, _vertex1, _vertex2;
        public Circle CircumCircle;

        public Triangle(Vector2 vertex0, Vector2 vertex1, Vector2 vertex2)
        {

            _vertex0 = vertex0;
            _vertex1 = vertex1;
            _vertex2 = vertex2;
            CircumCircle = CalcCircumCirc(vertex0, vertex1, vertex2);
        }

        public Circle CalcCircumCirc(Vector2 vertex0, Vector2 vertex1, Vector2 vertex2)
        {

            float x1 = vertex0.x, y1 = vertex0.y;
            float x2 = vertex1.x, y2 = vertex1.y;
            float x3 = vertex2.x, y3 = vertex2.y;

            float determinant = 2 * x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2);

            float Ux = ((x1 * x1 + y1 * y1) * (y2 - y3) + (x2 * x2 + y2 * y2) * (y3 - y1) +
                        (x3 * x3 + y3 * y3) * (y1 - y2)) / determinant;
            float Uy = ((x1 * x1 + y1 * y1) * (x3 - x2) + (x2 * x2 + y2 * y2) * (x1 - x3) +
                        (x3 * x3 + y3 * y3) * (x2 - x1)) / determinant;

            Vector2 circumcenter = new Vector2(Ux, Uy);
            float radius = Vector2.Distance(circumcenter, vertex0);
            return new Circle(circumcenter, radius);
        }
        
        public bool InCircumcircle(Vector2 point)
        {
            if (CircumCircle == null)
            {
                return false; 
            }

            float dx = CircumCircle.Center.x - point.x;
            float dy = CircumCircle.Center.y - point.y;

           
            float distSquared = dx * dx + dy * dy;
            float radiusSquared = CircumCircle.Radius * CircumCircle.Radius;
            
            return distSquared <= radiusSquared;
        }



        public class Circle
        {

            public Vector2 Center;
            public float Radius;

            public Circle(Vector2 center, float radius)
            {

                Center = center;
                Radius = radius;
            }
        }
    }
}   

   

