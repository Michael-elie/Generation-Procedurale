
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class DelauneyTriangulator 
    {
        public List<Triangle> triangles = new List<Triangle>();

      
        public void Triangulate(List<Room> rooms, Room initialRoom)
        {
            SuperTriangle superTriangle = new SuperTriangle(initialRoom);
            triangles.Add(superTriangle.triangle);  // ajoute le super triangle a la triangumation 
            
          
            foreach (Room room in rooms)
            {
                Vector2 center = room.GetCenter();
                AddPoint(center);
            }

            foreach (Triangle triangle in triangles)
            {
                
            }
            
            
           
        }

        private void AddPoint(Vector2 point)
        {
            List<Triangle> badTriangles = new List<Triangle>();
            foreach (Triangle triangle in triangles)
            {
                if (triangle.InCircumcircle(point))
                {
                    badTriangles.Add(triangle);
                }
            }
            
            List<Edge> polygonEdges = new List<Edge>();
            foreach (Triangle triangle in badTriangles)
            {
                foreach (Edge edge in GetTriangleEdges(triangle))
                {
                   
                  /* if ( if  not shared by any other triangles in bad trinagles ) 
                    {
                        polygonEdges.Add(edge);
                    }     */
                }
            }

            foreach (Triangle triangle in badTriangles) {
                triangles.Remove(triangle);
            }
            
            foreach (Edge edge in polygonEdges)
            {
                Triangle newTriangle = new Triangle(edge.Vertex0, edge.Vertex1, point);
                triangles.Add(newTriangle);
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

        
        public void DrawTriangles()
        {
            foreach (Triangle triangle in triangles)
            {
                Debug.DrawLine(triangle.Vertex0, triangle.Vertex1, Color.red,500);
                Debug.DrawLine(triangle.Vertex1, triangle.Vertex2, Color.red,500);
                Debug.DrawLine(triangle.Vertex2, triangle.Vertex0, Color.red,500);
            }
        }
    }
    
   
    
 