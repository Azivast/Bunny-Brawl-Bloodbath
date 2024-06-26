using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace ProceduralGeneration
{
    public class Agent
    {
        public int MaxSteps;
        public int ChangeDirectionChance;
        public int AddRoomChance;
        public int NewAgentChance;


        public Vector2Int Direction;
        public Vector2Int Position;

        private Vector2Int levelBounds;
        private int stepsTaken = 0;

        public Agent(Vector2Int pos, int maxSteps, int directionChance, int roomChance, int newAgentChance, Vector2Int levelBounds)
        {
            ChangeDirectionChance = directionChance;
            AddRoomChance = roomChance;
            MaxSteps = maxSteps;
            NewAgentChance = newAgentChance;
            Position = pos;
            this.levelBounds = levelBounds;
            RandomizeDirection();
        }

        /// <summary>
        /// Returns true if agent has reached the maximum steps
        /// </summary>
        /// <returns></returns>
        public bool Move()
        {
            while (!CheckMove())
            {
                RandomizeDirection();
            }
            if (stepsTaken > MaxSteps)
            {
                return false;
            }
            else
            {
                Position += Direction;
                stepsTaken++;
                return true;
            }
        }

        private bool CheckMove()
        {
            Vector2Int newPos = Position + Direction;
            
            if (newPos.x >= levelBounds.x-1 || newPos.x < 1 ||
                newPos.y >= levelBounds.y-1 || newPos.y < 1)
            {

                return false;
            }
            else
            {
                return true;
            }
        }


        public bool RandomizeDirection()
        {
            Vector2Int newDirection = ConstRandom.Random.Next(4) switch
            {
                0 => Vector2Int.left,
                1 => Vector2Int.up,
                2 => Vector2Int.right,
                3 => Vector2Int.down,
                _ => throw new Exception("random integer out of range")
            };
            if (newDirection + Direction == Vector2Int.zero) // 180 degree turn
            {
                Direction = newDirection;
                return true;
            }
            else
            {
                Direction = newDirection;
                return false;
            }
        }
    }
}
