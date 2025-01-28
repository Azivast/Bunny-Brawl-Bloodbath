using System.Collections;
using System.Collections.Generic;
using ProceduralGeneration;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;

#if UNITY_EDITOR // => Ignore from here to next endif if not in editor
    [CustomEditor(typeof(PerlinNoiceTerrain))]
    public class PerlinNoiceTerrainEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate"))
            {
                PerlinNoiceTerrain generator = target as PerlinNoiceTerrain;
                generator.Generate();
            }
                
            if (GUILayout.Button("Randomize New"))
            {
                PerlinNoiceTerrain generator = target as PerlinNoiceTerrain;
                generator.GenerateNew();
            }
        }
    }
#endif
public class PerlinNoiceTerrain : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private Vector2Int levelCapacity = new Vector2Int(1000,1000);
    [SerializeField] private int seed = 1234567;
    [SerializeField] private bool randomizeSeed;
    [Header("Components")]
    [SerializeField] private TilemapPopulator tilemapPopulator = new TilemapPopulator();
    [SerializeField] private LevelChestSpawner chestSpawner = new LevelChestSpawner();
    [SerializeField] private LevelEnemySpawner enemySpawner = new LevelEnemySpawner();

    [Header("Perlin Noise")]
    [SerializeField] private float scale = 1.0f;
    [FormerlySerializedAs("thershHold")] [SerializeField] private float threshold = 0.5f;
    
    [SerializeField]float strenght = 0.01f;
    [SerializeField]float guaranteedPlayableRadius = 20f;


    
    private LevelGenerator.AvailableTiles[,] generatedLevel;
    
    private List<Vector2> weaponChestPostitions;
    private List<Vector2> ammoChestPositions;
    private List<Vector2> possibleChestSpawn;
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
        generatedLevel = new LevelGenerator.AvailableTiles[levelCapacity.x, levelCapacity.y];
        
        weaponChestPostitions = new List<Vector2>();
        ammoChestPositions = new List<Vector2>();
        chestSpawner.ClearObjects();
        enemySpawner.ClearEnemies();
    }
    
    public void Generate()
    {
        possibleChestSpawn = new List<Vector2>();
        ResetGeneration();
        
        CalcNoise();
        FloodFill();
        CalculateChestPositions();

        // Populate Tilemap
        tilemapPopulator.Populate(generatedLevel);
        
        //Debug.Log(chestSpawner);
        // Spawn Items
        chestSpawner.SpawnAmmoChests(ammoChestPositions, spawnPosition, seed);
        chestSpawner.SpawnWeaponChests(weaponChestPostitions, spawnPosition, seed);
        
        // Spawn Enemies
        enemySpawner.SpawnEnemies(generatedLevel, spawnPosition, tilemapPopulator.TileMiddleOffset);
    }

    void CalcNoise()
    {
        //985860
        Random.InitState(seed);
        ObjectDropper.SetSeed(seed);
        // For each pixel in the texture...
        for (int y = 0; y < levelCapacity.y; y++)
        {
            for (int x = 0; x < levelCapacity.x; x++)
            {
                float sample = Mathf.PerlinNoise(seed+x / scale, seed +y/scale);
                
                if ((levelCapacity / 2 - new Vector2Int(x, y)).magnitude > guaranteedPlayableRadius)
                {
                    sample += (levelCapacity / 2 - new Vector2Int(x, y)).magnitude * strenght;
                }
                if (sample < threshold) generatedLevel[x, y] = LevelGenerator.AvailableTiles.Ground;
                else generatedLevel[x, y] = LevelGenerator.AvailableTiles.Wall;
            }
        }
    }

    private void FloodFill()
    {
        int width = levelCapacity.x;
        int height = levelCapacity.y;

        // Get the center of the terrain
        Vector2Int center = new Vector2Int(width / 2, height / 2);
        
        // Create a queue for BFS (Queue of Vector2 positions to check)
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[,] visited = new bool[width, height];  // To track which cells have been visited

        //TODO: Fixa så att den hittar en annan punkt om mittpunkten inte är ground
        
        // Start flood fill from the center if it meets the threshold
        if (generatedLevel[center.x, center.y] == LevelGenerator.AvailableTiles.Ground)
        {
            queue.Enqueue(center);
            visited[center.x, center.y] = true;
            spawnPosition = new Vector2(center.x, center.y);
        }
        else
        {
            GenerateNew();
            Debug.Log("Wall on center, generating new");
            return;
        }

        // Directions for 4-way flood fill (up, down, left, right)
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            // Loop through each direction
            foreach (var direction in directions)
            {
                Vector2Int neighbor = current + direction;

                // Check if the neighbor is within bounds and if it's a valid tile to floodfill
                if (neighbor.x >= 0 && neighbor.x < width && neighbor.y >= 0 && neighbor.y < height)
                {
                    if (!visited[neighbor.x, neighbor.y] &&
                        generatedLevel[neighbor.x, neighbor.y] == LevelGenerator.AvailableTiles.Ground)
                    {
                        visited[neighbor.x, neighbor.y] = true;
                        queue.Enqueue(neighbor);
                    }

                    if (!visited[neighbor.x, neighbor.y] &&
                        generatedLevel[neighbor.x, neighbor.y] == LevelGenerator.AvailableTiles.Wall)
                    {
                        possibleChestSpawn.Add(new Vector2(current.x + 0.5f, current.y + 0.5f));
                    }
                }
            }
        }

        for (int y = 0; y < levelCapacity.y; y++)
        {
            for (int x = 0; x < levelCapacity.x; x++)
            {
                if (!visited[x, y]) generatedLevel[x, y] = LevelGenerator.AvailableTiles.Wall;
            }
        }
        // After flood fill, you can mark the filled area or modify it in some way.
        Debug.Log("Flood fill complete.");
    }

    private void CalculateChestPositions()
    {
        int count = possibleChestSpawn.Count;
        Debug.Log("Possible position count: " + count);
        for (int i = 0; i < count; i++)
        {
            int number = Random.Range(0, possibleChestSpawn.Count);
            if(i % 2 == 0)
            {
                ammoChestPositions.Add(possibleChestSpawn[number]);
            }
            else
            {
                weaponChestPostitions.Add(possibleChestSpawn[number]);
            }
            possibleChestSpawn.RemoveAt(number);
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
