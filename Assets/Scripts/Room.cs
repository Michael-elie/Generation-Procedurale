using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Room
{
   
   public int Widht;
   public int Height;
   public Vector2Int Position;
  // public Vector2Int CenterPosition;
   public Room(Vector2Int position,int widht, int height)
   {
      Widht = widht;
      Height = height;
      Position = position;
      //CenterPosition = centerPoint;
   }
   
   
   
   
}
