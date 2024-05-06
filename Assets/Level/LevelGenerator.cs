using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SearchService;
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
        private List<Vector2Int> itemPositions;
        private Vector2Int spawnPosition;

        private void Start()
        {
            GenerateNew();
        }

        public void GenerateNew()
        {
            PrepareGeneration();
            Generate();
        }

        private void PrepareGeneration()
        {
            seed = UnityEngine.Random.Range(0, 1000000);
        }
        
        public void Generate()
        {
            // Setup
            random = new Random(seed);
            generatedLevel = new AvailableTiles[levelCapacity.x, levelCapacity.y]; // todo: prettify this
            startingTile.x = levelCapacity.x/2;
            startingTile.y = levelCapacity.y/2;
            itemPositions = new List<Vector2Int>();
            
            // Create agent
            Agent agent = new Agent(startingTile, levelCapacity, random);
            // Place starting tile
            generatedLevel[agent.Position.x, agent.Position.y] = AvailableTiles.Ground;
            spawnPosition = agent.Position;

            for (int i = 0; i < agentSteps; i++)
            {
                agent.Move();
                
                // Place tile
                generatedLevel[agent.Position.x, agent.Position.y] = AvailableTiles.Ground; // replace with function that verifies if within array

                // Change direction?
                int newDirectionChance = random.Next(100);
                if (newDirectionChance < agent.ChangeDirectionChance)
                {
                    agent.ChangeDirectionChance = 0;
                    if (agent.RandomizeDirection()) // if 180 degree turn, save position as potential item spawn point
                    {
                        itemPositions.Add(agent.Position);
                    }
                }
                else
                {
                    agent.ChangeDirectionChance += 5;
                }
                
                // Place Room?
                int newRoomChance = random.Next(100);
                if (newRoomChance < agent.AddRoomChance)
                {
                    Vector2Int roomSize = new Vector2Int(random.Next(minRoomSize, maxRoomSize + 1), random.Next(minRoomSize, maxRoomSize + 1));
                    Vector2Int startPos = agent.Position - (roomSize / 2);

                    // Place room centered around agent
                    for (int y = 0; y < roomSize.y; y++)
                    {
                        for (int x = 0; x <  roomSize.x; x++)
                        {
                            TryPlaceTile(startPos + new Vector2Int(x, y), AvailableTiles.Ground);
                        }
                    }
                    
                    agent.AddRoomChance = 0;
                }
                else
                {
                    agent.AddRoomChance += 5;
                }
            }
            
            // Populate Tilemap
            tilemapPopulator.Populate(generatedLevel);
            
            // Spawn Items
            SpawnItems(itemPositions);
        }

        private bool TryPlaceTile(Vector2Int position, AvailableTiles type)
        {
            if (position.x >= levelCapacity.x-1 || position.x < 1 ||
                position.y >= levelCapacity.y-1 || position.y < 1)
            {
                return false;
            }
            else
            {
                generatedLevel[position.x, position.y] = type;
                return true;
            }
        }

        private void SpawnItems(List<Vector2Int> spawnPositions)
        {

        }

        private void OnDrawGizmos()
        {
            // Item spots
            foreach (var pos in itemPositions)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(pos.x, pos.y, 0), 1);
            }

            // Player spawn
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(spawnPosition.x, spawnPosition.y, 0), 1);
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
                LevelGenerator generator = target as LevelGenerator;
                generator.Generate();
            }
            
            if (GUILayout.Button("Randomize New"))
            {
                LevelGenerator generator = target as LevelGenerator;
                generator.GenerateNew();
            }
        }
    }
}

