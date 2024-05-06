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

        private Vector2Int levelBounds;
        private Random random;

        public Agent(Vector2Int pos, Vector2Int levelBounds, Random random)
        {
            Position = pos;
            this.levelBounds = levelBounds;
            this.random = random;
            RandomizeDirection();
        }

        public void Move()
        {
            while (!CheckMove())
            {
                RandomizeDirection();
            }
            Position += Direction;
        }

        private bool CheckMove()
        {
            Vector2Int newPos = Position + Direction;
            
            if (newPos.x >= levelBounds.x || newPos.x < 0 ||
                newPos.y >= levelBounds.y || newPos.y < 0)
            {

                return false;
            }
            else
            {
                return true;
            }
        }

        public void RandomizeDirection()
        {
            Direction = random.Next(4) switch
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
