using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class BspGenerator : MonoBehaviour
{
  [SerializeField] private int maxRooms = 10;
  [SerializeField] private int _seed;
  private Room _firstRoom;
  private System.Random rnd;
  
  private void Start()
  {
    rnd = new System.Random(_seed);
    _firstRoom = new Room(20,20,Vector2.zero);
  }
  

  public List<Room> _rooms(List<Room> rooms, int maxRooms)
  {
    
    return rooms;
  }
  
  public void Split() {
    
  }
  
  public void HorizontalSplit() {
    // sur la height
    int splitPoint = rnd.Next();
  }
  
  public void VerticalSplit() {
    // sur la Widht
  }
  
  
  
  
  
  
  
}
