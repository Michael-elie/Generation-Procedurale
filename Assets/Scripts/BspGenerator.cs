  using System;
  using System.CodeDom.Compiler;
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.Tilemaps;
  using Random = UnityEngine.Random;


  public class BspGenerator : MonoBehaviour
  {
    public List<Room> rooms = new List<Room>();
    [SerializeField] private int maxRooms = 10;
    [SerializeField] private int _seed;
    [SerializeField] float minRoomSize = 3f;
    //private Room _firstRoom;
    private System.Random rnd;
    private bool _lastSplitWasHorizontal;
    
    private void Start() {
     
      Room initialRoom = new Room(new Vector2Int(0, 0),50, 50);
      rooms.Add(initialRoom);
      GenerateRooms(initialRoom);
     
    }


    private void GenerateRooms(Room room) {
     
      /*Room initialRoom = new Room(new Vector2Int(0, 0),100, 50 );
      rooms.Add(initialRoom);*/

      if (room.Widht <= minRoomSize || room.Height <= minRoomSize) {
        return; // stop si la salle est trop petite
      }
      
      /*if (rooms.Count >= 10)
      {
        return;
      }*/
      
      List<Room> newRooms = _rooms(room);
      rooms.Remove(room);
      
      foreach (Room newRoom in newRooms) {
        rooms.Add(newRoom);
        //GenerateRooms(newRoom);
      }
    
      
    }
    

    public List<Room> _rooms(Room room) {
    
      rnd = new System.Random(_seed);
      List<Room> newRooms = new List<Room>();

      bool splitHonrizontaly = rnd.Next(0, 2) == 0; //2 exclus 
     // splitHonrizontaly = !_lastSplitWasHorizontal;

      if (splitHonrizontaly) {
       // int splitWidth = room.Widht / 2;
       int splitWidth = rnd.Next(1, (room.Widht ));
        Room room1 = new Room(room.Position, splitWidth, room.Height);
        Room room2 = new Room(new Vector2Int(room.Position.x + splitWidth, room.Position.y), splitWidth, room.Height);
        newRooms.Add(room1);
        newRooms.Add(room2);
      }
      else {
       // int splitHeight = room.Height / 2;  
       int splitHeight = rnd.Next(1, (room.Height ));
        Room room1 = new Room(room.Position, room.Widht, splitHeight);
        Room room2 = new Room(new Vector2Int(room.Position.x, room.Position.y + splitHeight), room.Widht, splitHeight);
        newRooms.Add(room1);
        newRooms.Add(room2);
        
      }
      
      _lastSplitWasHorizontal = splitHonrizontaly;
      return newRooms;
    }
    
    
   /* public void HorizontalSplit(List<Room> newRooms) {
      // sur la height
      
      int splitPoint = rnd.Next();
      
      Room room1 = new Room(
      
    }
    
    public void VerticalSplit() {
      // sur la Widht
    }*/
    
    
    
     void OnDrawGizmos()
     {
       Gizmos.color = Color.red;

       // Dessine chaque salle dans la liste
       foreach (Room room in rooms)
       {
         Gizmos.DrawWireCube(new Vector3(room.Position.x + room.Widht / 2, room.Position.y + room.Height / 2, 0), new Vector3(room.Widht, room.Height, 0));
       }
     }
    
    
    
  }
