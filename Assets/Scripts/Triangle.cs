using System.Collections.Generic;
using UnityEngine;


    public class Triangle
    {
        public Vector2 v1, v2, v3;

        public Triangle(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public bool ContainsVertex(Vector2 v)
        {
            return v == v1 || v == v2 || v == v3;
        }

        public bool IsPointInsideCircumcircle(Vector2 point)
        {
            float ax = v1.x - point.x;
            float ay = v1.y - point.y;
            float bx = v2.x - point.x;
            float by = v2.y - point.y;
            float cx = v3.x - point.x;
            float cy = v3.y - point.y;

            float det = (ax * ax + ay * ay) * (bx * cy - cx * by) -
                        (bx * bx + by * by) * (ax * cy - cx * ay) +
                        (cx * cx + cy * cy) * (ax * by - bx * ay);

            return det > 0;
        }

        public List<Edge> GetEdges()
        {
            return new List<Edge>
            {
                new Edge(v1, v2),
                new Edge(v2, v3),
                new Edge(v3, v1)
            };
        }

        public bool ContainsEdge(Edge edge)
        {
            return (v1 == edge.v1 && v2 == edge.v2) || (v2 == edge.v1 && v3 == edge.v2) ||
                   (v3 == edge.v1 && v1 == edge.v2);
        }
    }

    public struct Edge
    {
        public Vector2 v1, v2;

        public Edge(Vector2 v1, Vector2 v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public bool Equals(Edge other)
        {
            return (v1 == other.v1 && v2 == other.v2) || (v1 == other.v2 && v2 == other.v1);
        }
    }


