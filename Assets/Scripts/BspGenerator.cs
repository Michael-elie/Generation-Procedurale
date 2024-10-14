using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = System.Random;


public class BspGenerator : MonoBehaviour
{
    public List<Room> Rooms = new List<Room>();
    public enum SplitDirection { Horizontal , Vertical  }
    public SplitDirection splitDirection;
    [SerializeField] private int initialHeightRoom = 50;
    [SerializeField] private int initialWidhtRoom = 100;
    [SerializeField] private int seed;
    [SerializeField] private int depth;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile floorTile;
    private Room _firstRoom;
    private Random _rnd;
    private Dictionary<Vector2, HashSet<Vector2>> roomConnections = new Dictionary<Vector2, HashSet<Vector2>>();


    [ContextMenu("BSP Generation")]
    public void BSP() {
        _firstRoom = new Room(new Vector2Int(0, 0), initialWidhtRoom, initialHeightRoom);
        Rooms.Add(_firstRoom);
        Debug.Log($"FisrtRoom width :{_firstRoom.Widht}, FirstRoom height : ,{_firstRoom.Height} ");
        RoomsGeneration(_firstRoom);
    }

    private void RoomsGeneration(Room room) {
      /*  if (room.Widht <= minRoomSize.x || room.Height <= minRoomSize.y) return;  stop si la salle est trop petite*/
        _rnd = new Random(seed);
        _roomsSplit(_firstRoom);
        _recursiveSplit(Rooms, depth);
        RoomVizualizer(Rooms);
    }

    public List<Room> _recursiveSplit(List<Room> rooms, int depth) {
        if (depth == 0) return rooms;
        // Find a room to cut
        //Room roomToCut = rooms[UnityEngine.Random.Range(0, rooms.Count)];
        Room roomToCut = FindTheBiggestRoom();
        // Cut the room
        List<Room> newRooms = _roomsSplit(roomToCut);
        // Remove the cutted room
        rooms.Remove(roomToCut);
        // Add the new rooms
        foreach (Room newRoom in newRooms)
        {
            Rooms.Add(newRoom);
            Debug.Log($"Room width :{newRoom.Widht}, Room height : ,{newRoom.Height} ,Room Position : ,{newRoom.Position}");
        }
        // Go to next iteration
        return _recursiveSplit(rooms, depth - 1);
    }
    
    public List<Room> _roomsSplit(Room room) {
        
        var newRooms = new List<Room>();
        
        
        if (splitDirection  == SplitDirection.Vertical) {
            // vercital slice 
            int cutValue = _rnd.Next(1, (room.Height * _rnd.Next(5,7)/ 10 ));
            var room1 = new Room(room.Position, room.Widht, cutValue);
            var room2 = new Room(new Vector2Int(room.Position.x, room.Position.y + cutValue), room.Widht,
                room.Height - cutValue);
            newRooms.Add(room1);
            newRooms.Add(room2);

            splitDirection = SplitDirection.Horizontal;
        }
        else {
            // horizontal slice 
            int cutValue = _rnd.Next(1, (room.Widht * _rnd.Next(5,7)/ 10 ));
            var room1 = new Room(room.Position, cutValue, room.Height);
            var room2 = new Room(new Vector2Int(room.Position.x + cutValue, room.Position.y), room.Widht - cutValue, room.Height);
            newRooms.Add(room1);
            newRooms.Add(room2);

            splitDirection = SplitDirection.Vertical;
        }
        
        return newRooms;
        
    }

    public Room FindTheBiggestRoom()
    {
        Room biggestRoom = new Room(Vector2Int.zero,0,0);
        foreach (var room in Rooms) {
            
            int roomArea = room.Widht * room.Height;
            int biggestRoomArea = biggestRoom.Widht * biggestRoom.Height;
            if (roomArea > biggestRoomArea) {
                biggestRoom = room;
            }
        }
        return biggestRoom;
        
    }
   private void RoomVizualizer(List<Room> rooms) {
       foreach (Room room in rooms) {
           Color randomColor = new Color((float)_rnd.NextDouble(), (float)_rnd.NextDouble(), (float)_rnd.NextDouble());
           
           for (int x = 0; x < room.Widht; x++) {
               
               for (int y = 0; y < room.Height; y++) {
                   
                   Vector3Int tilePosition = new Vector3Int(room.Position.x + x, room.Position.y + y, 0);
                   tilemap.SetTile(tilePosition, floorTile);
                   tilemap.SetTileFlags(tilePosition, TileFlags.None);
                   tilemap.SetColor(tilePosition, randomColor);
               }
           }
       }
      
    }
   
 public void BowyerWatsonTriangulation() {
        List<Vector2> points = new List<Vector2>();
        foreach (Room room in Rooms) {
            points.Add(room.GetCenter());
        }

        float maxX = 1000, maxY = 1000;
        Triangle superTriangle = new Triangle(new Vector2(-maxX, -maxY), new Vector2(maxX, -maxY), new Vector2(0, maxY * 2));
        List<Triangle> triangles = new List<Triangle> { superTriangle };

        foreach (Vector2 point in points) {
            List<Triangle> badTriangles = new List<Triangle>();
            List<Edge> polygon = new List<Edge>();

            foreach (Triangle triangle in triangles) {
                if (triangle.IsPointInsideCircumcircle(point)) {
                    badTriangles.Add(triangle);
                }
            }

            foreach (Triangle triangle in badTriangles) {
                foreach (Edge edge in triangle.GetEdges()) {
                    bool isShared = false;
                    foreach (Triangle other in badTriangles) {
                        if (triangle != other && other.ContainsEdge(edge)) {
                            isShared = true;
                            break;
                        }
                    }
                    if (!isShared) {
                        polygon.Add(edge);
                    }
                }
            }

            triangles.RemoveAll(t => badTriangles.Contains(t));

            foreach (Edge edge in polygon) {
                triangles.Add(new Triangle(edge.v1, edge.v2, point));
                AddConnection(edge.v1, edge.v2);
            }
        }

        triangles.RemoveAll(t => t.ContainsVertex(superTriangle.v1) || t.ContainsVertex(superTriangle.v2) || t.ContainsVertex(superTriangle.v3));

        foreach (Triangle triangle in triangles) {
            GenerateCorridor(triangle.v1, triangle.v2);
            GenerateCorridor(triangle.v2, triangle.v3);
            GenerateCorridor(triangle.v3, triangle.v1);
        }
    }

    private void AddConnection(Vector2 room1, Vector2 room2) {
        if (!roomConnections.ContainsKey(room1)) {
            roomConnections[room1] = new HashSet<Vector2>();
        }
        if (!roomConnections.ContainsKey(room2)) {
            roomConnections[room2] = new HashSet<Vector2>();
        }
        roomConnections[room1].Add(room2);
        roomConnections[room2].Add(room1);
    }

    private void GenerateCorridor(Vector2 start, Vector2 end) {
        Vector2Int current = new Vector2Int((int)start.x, (int)start.y);
        Vector2Int target = new Vector2Int((int)end.x, (int)end.y);

        while (current.x != target.x) {
            current.x += (target.x > current.x) ? 1 : -1;
            tilemap.SetTile(new Vector3Int(current.x, current.y, 0), floorTile);
        }
        while (current.y != target.y) {
            current.y += (target.y > current.y) ? 1 : -1;
            tilemap.SetTile(new Vector3Int(current.x, current.y, 0), floorTile);
        }
    }

    
    
    
}