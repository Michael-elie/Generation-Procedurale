
    using System.Collections.Generic;
    using UnityEngine;

    public class DelauneyTriangulator
    {
        private List<Triangle> triangles = new List<Triangle>();

        public void Triangulate(List<Room> rooms)
        {
           // SuperTriangle superTriangle = new SuperTriangle();
          //  triangles.Add(superTriangle.triangle);
            
            
            foreach (var room in rooms)
            {
                Vector2 center = room.GetCenter();
                AddPoint(center);
            }
        }

        private void AddPoint(Vector2 point)
        {
            List<Triangle> badTriangles = new List<Triangle>();
            foreach (var triangle in triangles)
            {
                if (triangle.InCircumcircle(point))
                {
                    badTriangles.Add(triangle);
                }
            }
        }

        private List<Edge> GetTriangleEdges(Triangle triangle)
        {
            return new List<Edge>
            {
                new Edge(triangle.Vertex0, triangle.Vertex2),
                new Edge(triangle.Vertex1, triangle.Vertex2),
                new Edge(triangle.Vertex2, triangle.Vertex0)
            };
        }

        
      /*  public void DrawTriangles()
        {
            foreach (var triangle in triangles)
            {
                Debug.DrawLine(triangle.Vertex0, triangle.Vertex1, Color.red,500);
                Debug.DrawLine(triangle.Vertex1, triangle.Vertex2, Color.red,500);
                Debug.DrawLine(triangle.Vertex2, triangle.Vertex0, Color.red,500);
            }
        }*/
    }
    
   
    
 