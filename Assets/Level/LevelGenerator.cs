using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

// drunkard walk algorithm
[ExecuteAlways]
public class LevelGenerator : MonoBehaviour {
    [SerializeField] private Tilemap floorTileMap, wallTileMap;
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile borderTile;
    [SerializeField] private int seed = 1234567;
    [SerializeField] private int MaxFloors = 110;
    [SerializeField] private float chanceOf2x2Room = 50;
    [SerializeField] private float chanceOf3x3Room = 11;
    [SerializeField] private float chanceOfNewFloorMaker = 1;
    [SerializeField] private float chanceOfFloorMakerDeath = 1;
    [SerializeField] private float chanceOfFloorMakerDeathIncrease = 1;
    [SerializeField] private GameObject ammoChest;
    [SerializeField] private GameObject weaponChest;
    [SerializeField] private ObjectCollection enemies;
    [SerializeField] private float enemiesToSpawn = 6;
    [SerializeField] private float safeZoneRadius = 6;

    private int wallPadding = 1;
    private int floorsPlaced;
    private List<Vector3Int> ammoChestList = new();
    private List<Vector3Int> weaponChestList = new();
    private List<Vector3Int> enemyList = new();

    private List<FloorMaker> floorMakers;
    private Random random;
    private float chanceOfFloorMakerDeathInternal;

    struct FloorMaker {
        public Vector2Int Dir;
        public Vector3Int Pos;
    }

    private void Start() {
        random = new Random(seed);
        Setup();
        GenerateFloors(chanceOf2x2Room, chanceOf3x3Room);
        GenerateWalls();
        SpawnChests();
        SpawnEnemies();
    }
    
    private Vector2Int RandomDirection() {
        int randInt = random.Next(4);

        Vector2Int direction = Vector2Int.zero;
        switch (randInt) {
            case 0: 
                direction = Vector2Int.up;
                break;
            case 1: 
                direction = Vector2Int.down;
                break;
            case 2: 
                direction = Vector2Int.left;
                break;
            case 3: 
                direction = Vector2Int.right;
                break;
            default: 
                Debug.LogWarningFormat("Invalid case");
                break;
        }

        return direction;
    }

    private void Setup()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        floorTileMap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
        floorsPlaced = 0;
        chanceOfFloorMakerDeathInternal = chanceOfFloorMakerDeath;
        floorMakers = new List<FloorMaker>();
        
        ammoChestList.Clear();
        weaponChestList.Clear();
        enemyList.Clear();
    }

    private void GenerateFloors(float chance2x2, float chance3x3) {
        // Spawn an initial floorMaker
        SpawnFloorMaker(Vector3Int.zero);
        // Spawn an initial floor
        floorTileMap.SetTile(Vector3Int.zero, floorTile);

        floorsPlaced = 0;
        do {
            for (int i = floorMakers.Count-1; i >= 0; i--) {
                var floorMaker = floorMakers[i];
                int rand;
                
                // STEP 1: Move floorMaker
                floorMaker.Pos += (Vector3Int)floorMaker.Dir;
                // spawn chest
                var newDirection =  RandomDirection();
                if (newDirection + floorMaker.Dir == Vector2Int.zero)
                {
                    weaponChestList.Add(floorMaker.Pos);
                }
                floorMaker.Dir = newDirection;
                floorMakers[i] = floorMaker;

                // chance of death
                rand = random.Next(0, 101);
                if (floorMakers.Count > 1 && rand < chanceOfFloorMakerDeathInternal)
                {
                    ammoChestList.Add(floorMaker.Pos);
                    floorMakers.Remove(floorMaker);
                }
                
                // STEP 2: Spawn floor
                if (!floorTileMap.HasTile(floorMaker.Pos)) floorsPlaced++;
                floorTileMap.SetTile(floorMaker.Pos, floorTile);


                // TODO STEP 2: Make rooms

                // STEP 3: Split corridors
                rand = random.Next(0, 10001);
                if (rand/10f < chanceOfNewFloorMaker)
                {
                    SpawnFloorMaker(floorMaker.Pos);
                    chanceOfFloorMakerDeathInternal += chanceOfFloorMakerDeathIncrease;
                }
            } 
        } while (floorsPlaced < MaxFloors);

        foreach (var maker in floorMakers)
        {
            ammoChestList.Add(maker.Pos);
        }
        
        floorTileMap.CompressBounds();
    }

    private void GenerateWalls() { //TODO: Optimize
        for (int i = floorTileMap.cellBounds.xMin-wallPadding; i <= floorTileMap.cellBounds.xMax+wallPadding; i++) {
            for (int j = floorTileMap.cellBounds.yMin-wallPadding; j <= floorTileMap.cellBounds.yMax+wallPadding; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                if (floorTileMap.HasTile(pos) && !floorTileMap.HasTile(pos + Vector3Int.up)) {
                    floorTileMap.SetTile(pos, borderTile);
                }
                else if (!floorTileMap.HasTile(pos))
                {
                    wallTileMap.SetTile(pos, wallTile);
                }
            } 
        }
        wallTileMap.CompressBounds();
    }

    private void SpawnFloorMaker(Vector3Int pos) {
        FloorMaker maker = new FloorMaker();
        maker.Pos = pos;
        maker.Dir = RandomDirection();

        floorMakers.Add(maker);
    }

    private void SpawnObject(Vector3Int pos, GameObject obj)
    {
        Instantiate(obj, (Vector3)pos + floorTileMap.cellSize/2, Quaternion.identity, transform);
    }

    private void SpawnChests()
    {
        RemoveButFurthest(ammoChestList);
        RemoveButFurthest(weaponChestList);
        
        SpawnObject(ammoChestList[0], ammoChest);
        SpawnObject(weaponChestList[0], weaponChest);
    }
    
    private void RemoveButFurthest(List<Vector3Int> positions)
    {
        Vector3Int furthest = positions[^1];
        for (int i = positions.Count-2; i > 0; i--)
        {
            if (positions[i + 1].magnitude > furthest.magnitude)
            {
                furthest = positions[i];
            }
        }
    }
    
    private void SpawnEnemies()
    {
        int enemiesSpawned = 0;
        
        while (enemiesSpawned < enemiesToSpawn)
        {
            int x, y;
            Vector3Int pos;
            do
            {
                x = random.Next(floorTileMap.cellBounds.xMin, floorTileMap.cellBounds.xMax);
                y = random.Next(floorTileMap.cellBounds.yMin, floorTileMap.cellBounds.yMax);
                pos = new Vector3Int(x, y, 0);
            } while (pos.magnitude < safeZoneRadius);
                 

            if (floorTileMap.HasTile(pos))
            {
                int index = random.Next(enemies.GetObjects().Count);
                SpawnObject(pos, enemies.GetObjects()[index]);
                enemiesSpawned++;
            }
        }
    }

    private void OnApplicationQuit() {
        //Quickfix TODO
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
