
    using System.Collections.Generic;
    using UnityEngine;

    public class DelauneyTriangulator
    {
        private List<Triangle> triangles = new List<Triangle>();

        public void Triangulate(List<Room> rooms)
        {
            SuperTriangle superTriangle = new SuperTriangle(rooms);
            triangles.Add(superTriangle.triangle);
            
            
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

            // Créer un ensemble d'arêtes à partir des triangles "mauvais"
            HashSet<Edge> polygonEdges = new HashSet<Edge>();
            foreach (var triangle in badTriangles)
            {
                // Ajouter les arêtes du triangle à l'ensemble
                foreach (var edge in GetTriangleEdges(triangle))
                {
                    if (!polygonEdges.Add(edge)) // Si l'arête existe déjà, l'enlever
                    {
                        polygonEdges.Remove(edge);
                    }
                }
            }

            // Retirer les triangles "mauvais" de la liste
            triangles.RemoveAll(t => badTriangles.Contains(t));

            // Créer de nouveaux triangles pour chaque arête de l'ensemble
            foreach (var edge in polygonEdges)
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
            foreach (var triangle in triangles)
            {
                Debug.DrawLine(triangle.Vertex0, triangle.Vertex1, Color.red,500);
                Debug.DrawLine(triangle.Vertex1, triangle.Vertex2, Color.red,500);
                Debug.DrawLine(triangle.Vertex2, triangle.Vertex0, Color.red,500);
            }
        }
    }
    
   
    
 