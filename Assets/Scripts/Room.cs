using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Room
{
   
   public float Widht;
   public float Height;
   public Vector2Int Position;  
   // public Vector2Int CenterPosition;
   public Room(Vector2Int position,float widht, float height) 
   {
      Widht = widht;
      Height = height;
      Position = position;
      // Color = color;
      //CenterPosition = centerPoint;
   }
   
   public Vector2 GetCenter() {
     return new Vector2(Position.x + Widht / 2, Position.y + Height / 2);
   }
   
}
