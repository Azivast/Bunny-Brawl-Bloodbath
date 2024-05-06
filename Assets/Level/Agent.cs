using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace ProceduralGeneration
{
    public class Agent
    {
        public int ChangeDirectionChance = 5;
        public int AddRoomChance = 5;

        public Vector2Int Direction;
        public Vector2Int Position;

        public Agent(Vector2Int dir, Vector2Int pos)
        {
            Direction = dir;
            Position = pos;
        }

        public void Move()
        {
            Position += Direction;
        }

        public void RandomizeDirection(Random rand)
        {
            Direction = rand.Next(4) switch
            {
                0 => Vector2Int.left,
                1 => Vector2Int.up,
                2 => Vector2Int.right,
                3 => Vector2Int.down,
                _ => throw new Exception("random integer out of range")
            };
        }
    }
}
