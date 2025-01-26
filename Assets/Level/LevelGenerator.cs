using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.Serialization;
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
        [Header("Level Settings")]
        [SerializeField] private Vector2Int levelCapacity = new Vector2Int(1000,1000);
        [SerializeField] private int numberOfFloors = 110;
        [SerializeField] private int minRoomSize = 3;
        [SerializeField] private int maxRoomSize = 7;
        [SerializeField] private int seed = 1234567;
        [SerializeField] private bool randomizeSeed;
        [Header("Agent Settings")]
        [SerializeField] private int agentDirectionChance = 5;
        [SerializeField] private int agentRoomChance = 5;
        [SerializeField] private int agentDuplicateChance = 1;
        [SerializeField] private int agentSteps = 10;
        [Header("Components")]
        [SerializeField] private TilemapPopulator tilemapPopulator = new TilemapPopulator();
        [SerializeField] private LevelChestSpawner chestSpawner = new LevelChestSpawner();
        [SerializeField] private LevelEnemySpawner enemySpawner = new LevelEnemySpawner();
        
        private Vector2Int startingTile;
        private AvailableTiles[,] generatedLevel;
        
        private List<Agent> agents = new List<Agent>();
        private int floorsPlaced;
        private List<Vector2> weaponChestPostitions;
        private List<Vector2> ammoChestPositions;
        private Vector2 spawnPosition;

        private void Start()
        {
            chestSpawner.ClearObjects();
            enemySpawner.ClearEnemies();
            GenerateNew();
        }

        private void OnDestroy()
        {
            chestSpawner.ClearObjects();
            enemySpawner.ClearEnemies();
        }

        public void GenerateNew()
        {
            if (randomizeSeed) RandomizeSeed();
            Generate();
        }

        private void RandomizeSeed()
        {
            seed = UnityEngine.Random.Range(0, 1000000);
        }

        private void ResetGeneration()
        {
            ConstRandom.Random = new Random(seed);
            UnityEngine.Random.InitState(seed);
            generatedLevel = new AvailableTiles[levelCapacity.x, levelCapacity.y];
            startingTile.x = levelCapacity.x/2;
            startingTile.y = levelCapacity.y/2;
            weaponChestPostitions = new List<Vector2>();
            ammoChestPositions = new List<Vector2>();
            floorsPlaced = 0;
            agents.Clear();
            chestSpawner.ClearObjects();
            enemySpawner.ClearEnemies();
        }
        
        public void Generate()
        {
            ResetGeneration();

            // Create agent
            Agent startAgent = new Agent(startingTile, 1000000, agentDirectionChance, agentRoomChance, agentDuplicateChance, levelCapacity);// todo replace 1000000 with proper variable
            agents.Add(startAgent);
            // Place starting tile
            generatedLevel[startAgent.Position.x, startAgent.Position.y] = AvailableTiles.Ground;
            spawnPosition = startAgent.Position + tilemapPopulator.TileMiddleOffset;

            while (floorsPlaced <= numberOfFloors)
            {
                for (var i = agents.Count-1; i >= 0; i--)
                {
                    var agent = agents[i];
                    if (agent.Move() == false) // max step already taken
                    {
                        weaponChestPostitions.Add(agent.Position + tilemapPopulator.TileMiddleOffset);
                        agents.Remove(agent);
                        continue;
                    }

                    // Place tile
                    if (generatedLevel[agent.Position.x, agent.Position.y] !=
                        AvailableTiles.Ground)
                    {
                        floorsPlaced++;
                        generatedLevel[agent.Position.x, agent.Position.y] =
                            AvailableTiles.Ground; // replace with function that verifies if within array

                    }

                    // Change direction?
                    int newDirectionChance = ConstRandom.Random.Next(100);
                    if (newDirectionChance < agent.ChangeDirectionChance)
                    {
                        agent.ChangeDirectionChance = 0;
                        if (agent.RandomizeDirection()) // if 180 degree turn, save position as potential ammoChest spawn point
                        {
                            ammoChestPositions.Add(agent.Position + tilemapPopulator.TileMiddleOffset);
                        }
                    }
                    else
                    {
                        agent.ChangeDirectionChance += agentDirectionChance;
                    }

                    // Place Room?
                    int newRoomChance = ConstRandom.Random.Next(100);
                    if (newRoomChance < agent.AddRoomChance)
                    {
                        Vector2Int roomSize = new Vector2Int(ConstRandom.Random.Next(minRoomSize, maxRoomSize + 1),
                            ConstRandom.Random.Next(minRoomSize, maxRoomSize + 1));
                        Vector2Int startPos = agent.Position - (roomSize / 2);

                        // Place room centered around agent
                        for (int y = 0; y < roomSize.y; y++)
                        {
                            for (int x = 0; x < roomSize.x; x++)
                            {
                                if (TryPlaceTile(startPos + new Vector2Int(x, y), AvailableTiles.Ground))
                                {
                                    floorsPlaced++;
                                }
                            }
                        }

                        agent.AddRoomChance = 0;
                    }
                    else
                    {
                        agent.AddRoomChance += agentRoomChance;
                    }

                    // Spawn new agent?
                    int newAgentChance = ConstRandom.Random.Next(100);
                    if (newAgentChance < agent.NewAgentChance)
                    {
                        // Spawn new agent.
                        Agent newAgent = new Agent(
                            agent.Position,
                            agentSteps,
                            agentDirectionChance,
                            agentRoomChance,
                            agentDuplicateChance,
                            levelCapacity);
                        agents.Add(newAgent);
                    }
                }
            }
            // Remove remaining agents
            for (var i = agents.Count - 1; i >= 0; i--)
            {
                weaponChestPostitions.Add(agents[i].Position + tilemapPopulator.TileMiddleOffset);
            }

            // Populate Tilemap
            tilemapPopulator.Populate(generatedLevel);
            
            // Spawn Items
            chestSpawner.SpawnAmmoChests(ammoChestPositions, spawnPosition);
            chestSpawner.SpawnWeaponChests(weaponChestPostitions, spawnPosition);
            
            // Spawn Enemies
            enemySpawner.SpawnEnemies(generatedLevel, spawnPosition, tilemapPopulator.TileMiddleOffset);
        }
        private bool TryPlaceTile(Vector2Int position, AvailableTiles type)
        {
            if (position.x >= levelCapacity.x-1 || position.x < 1 || // out of bounds
                position.y >= levelCapacity.y-1 || position.y < 1)
            {
                return false;
            }
            else if (generatedLevel[position.x, position.y] == type) // already placed
            {
                return false;
            }
            else // otherwise place tile
            {
                generatedLevel[position.x, position.y] = type;
                return true;
            }
        }

        private void OnDrawGizmos()
        {
            // Player spawn
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(spawnPosition.x, spawnPosition.y, 0), 0.5f);
            
            // Weapon chests
            if (weaponChestPostitions != null)
            {
                foreach (var pos in weaponChestPostitions)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(new Vector3(pos.x, pos.y, 0), 0.5f);
                }
            }
            
            // Ammo chests
            if (ammoChestPositions != null)
            {
                foreach (var pos in ammoChestPositions)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(new Vector3(pos.x, pos.y, 0), 0.5f);
                }
            }
        }
    }
}

