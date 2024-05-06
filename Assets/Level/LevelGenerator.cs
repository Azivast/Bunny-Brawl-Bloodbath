using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace ProceduralGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        public enum AvailableTiles
        {
            Wall,
            Ground,
        }
        [SerializeField] private Vector2Int levelCapacity = new Vector2Int(1000,1000);
        [SerializeField] private int minRoomSize = 3;
        [SerializeField] private int maxRoomSize = 7;
        [SerializeField] private int agentSteps = 10;
        [SerializeField] private int seed = 1234567;
        [SerializeField] private TilemapPopulator tilemapPopulator = new TilemapPopulator();
        
        private Random random;
        private Vector2Int startingTile;
        private AvailableTiles[,] generatedLevel;

        private void Start()
        {
            PrepareGeneration();
            Generate();
        }

        private void PrepareGeneration()
        {
            generatedLevel = new AvailableTiles[levelCapacity.x, levelCapacity.y]; // todo: prettify this
            random = new Random(seed);
            // Start at center 
            startingTile.x = levelCapacity.x/2;
            startingTile.y = levelCapacity.y/2;
        }

        private void Generate()
        {
            Agent agent = new Agent(Vector2Int.right, startingTile);
            // Place starting tile
            generatedLevel[agent.Position.x, agent.Position.y] = AvailableTiles.Ground;

            for (int i = 0; i < agentSteps; i++)
            {
                agent.Move();
                
                // Place tile
                generatedLevel[agent.Position.x, agent.Position.y] = AvailableTiles.Ground;
                
                // Change direction?
                int newDirectionChance = random.Next(101); //exclusive upper bound
                if (newDirectionChance < agent.ChangeDirectionChance)
                {
                    agent.RandomizeDirection(random);
                    agent.ChangeDirectionChance = 0;
                }
                else
                {
                    agent.ChangeDirectionChance += 5;
                }
                
                // Place Room?
                int newRoomChance = random.Next(101);
                if (newRoomChance < agent.AddRoomChance)
                {
                    int roomWidth = random.Next(minRoomSize, maxRoomSize + 1);
                    int roomLength = random.Next(minRoomSize, maxRoomSize + 1);
                    //todo: place room around agent
                    agent.AddRoomChance = 0;
                }
                else
                {
                    agent.AddRoomChance += 5;
                }
            }
            
            // Populate Tilemap
            tilemapPopulator.Populate(generatedLevel);
        }
    }

    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGenerationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate"))
            {
                // todo: implement generate button
            }
        }
    }
}

