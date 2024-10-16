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

    }

    public class Edge // aretes
        {
            public Vector2 Vertex0, Vertex1;
            public Edge(Vector2 vertex0, Vector2 vertex1) {
                Vertex0 = vertex0;
                Vertex1 = vertex1;
            }
            public bool Equals(Edge otherEdge) {
                return (Vertex0 == otherEdge.Vertex0 && Vertex1 == otherEdge.Vertex1) ||
                       (Vertex0 == otherEdge.Vertex1 && Vertex1 == otherEdge.Vertex0);
            }
        }

    public class Triangle
    {
        public Vector2 Vertex0, Vertex1, Vertex2;
        public Circle CircumCircle;

        public Triangle(Vector2 vertex0, Vector2 vertex1, Vector2 vertex2)
        {

            Vertex0 = vertex0;
            Vertex1 = vertex1;
            Vertex2 = vertex2;
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
    
   

    public class SuperTriangle
    {
        public Triangle triangle;

        public SuperTriangle(List<Room> rooms)
        {
            float minx = Mathf.Infinity, miny = Mathf.Infinity;
            float maxx = -Mathf.Infinity, maxy = -Mathf.Infinity;

            // Trouver les coordonnées min et max des centres de salles
            foreach (var room in rooms)
            {
                Vector2 center = room.GetCenter();
                minx = Mathf.Min(minx, center.x);
                miny = Mathf.Min(miny, center.y);
                maxx = Mathf.Max(maxx, center.x);
                maxy = Mathf.Max(maxy, center.y);
            }

            // Créer le super triangle autour des coordonnées des salles
            float dx = maxx - minx;
            float dy = maxy - miny;
            float padding = 10f; // Espace pour s'assurer que le super triangle est assez grand
            triangle = new Triangle(
                new Vector2(minx - padding, miny - padding),
                new Vector2(maxx + padding, miny - padding),
                new Vector2((minx + maxx) / 2, maxy + padding)
            );
        }
    }
       

