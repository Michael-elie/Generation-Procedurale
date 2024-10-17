using System;
using System.Collections.Generic;
using UnityEngine;
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
    private DelauneyTriangulator delaunayTriangulator;
    
    [ContextMenu("BSP Generation")]
    public void BSP() {
        _rnd = new Random(seed);
        _firstRoom = new Room(new Vector2Int(0, 0), initialWidhtRoom, initialHeightRoom);
        Rooms.Add(_firstRoom);
        Debug.Log($"FisrtRoom width :{_firstRoom.Widht}, FirstRoom height : ,{_firstRoom.Height} ");
        RoomsGeneration(_firstRoom);
        
        SuperTriangle superTriangle = new SuperTriangle(_firstRoom);
        delaunayTriangulator = new DelauneyTriangulator();
        delaunayTriangulator.Triangulate(Rooms); 
       // delaunayTriangulator.DrawTriangles();
    }

    private void RoomsGeneration(Room room) {
      /*  if (room.Widht <= minRoomSize.x || room.Height <= minRoomSize.y) return;  stop si la salle est trop petite*/
        
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
   
   
   
   
   
   
   
   
   
   
   
}  

