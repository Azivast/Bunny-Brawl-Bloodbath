using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

// drunkard walk algorithm
[ExecuteAlways]
public class LevelGenerator : MonoBehaviour {
    [SerializeField] private Tilemap floorTileMap, wallTileMap;
    [SerializeField] private readonly Tile floorTile;
    [SerializeField] private readonly Tile wallTile;
    [SerializeField] private int seed = 1234567;
    [SerializeField] private int iterations = 10;
    [SerializeField] private float chanceOf2x2Room = 50;
    [SerializeField] private float chanceOf3x3Room = 11;
    [SerializeField] private float chanceOfNewFloorMaker = 1;
    [SerializeField] private float chanceOfFloorMakerDeath = 1;
    [SerializeField] private float chanceOfFloorMakerDeathIncrease = 1;

    enum tileSet {
        empty,
        floor,
        wall
    }

    private List<FloorMaker> floorMakers;
    private Random random;
    private float chanceOfFloorMakerDeathInternal;

    struct FloorMaker {
        public Vector2Int Dir;
        public Vector3Int Pos;
    }

    private void OnValidate() {
        //TODO: validate
        Start();
    }

    private void Start() {
        random = new Random(seed);
        Setup();
        GenerateFloors(iterations, chanceOf2x2Room, chanceOf3x3Room);
        GenerateWalls();
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

    private void Setup() {
        floorTileMap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
        
        chanceOfFloorMakerDeathInternal = chanceOfFloorMakerDeath;
        floorMakers = new List<FloorMaker>();
    }

    private void GenerateFloors(int iterations, float chance2x2, float chance3x3) {
        // Spawn an initial floorMaker
        SpawnFloorMaker(Vector3Int.zero);

        int currentIteration = 0;
        do {
            for (int i = floorMakers.Count-1; i >= 0; i--) {
                var floorMaker = floorMakers[i];
                int rand;
                
                // STEP 1: Spawn floor
                floorTileMap.SetTile(floorMaker.Pos, floorTile);

                // TODO STEP 2: Make rooms

                // STEP 3: Split corridors
                rand = random.Next(0, 10001);
                if (rand/10f < chanceOfNewFloorMaker)
                {
                    SpawnFloorMaker(floorMaker.Pos);
                    chanceOfFloorMakerDeathInternal += chanceOfFloorMakerDeathIncrease;
                }
                
                // Move floorMaker
                floorMaker.Pos += (Vector3Int)floorMaker.Dir;
                floorMaker.Dir = RandomDirection();
                floorMakers[i] = floorMaker;

                // chance of death
                rand = random.Next(0, 101);
                if (floorMakers.Count > 1 && rand < chanceOfFloorMakerDeathInternal)
                {
                    floorMakers.Remove(floorMaker);
                }
            } 
        } while (currentIteration++ < iterations);
        
        floorTileMap.CompressBounds();
    }

    private void GenerateWalls() { //TODO: Optimize
        for (int i = floorTileMap.cellBounds.xMin; i <= floorTileMap.cellBounds.xMax; i++) {
            for (int j = floorTileMap.cellBounds.yMin; j <= floorTileMap.cellBounds.yMax; j++) {
                if (wallTileMap.GetTile(new Vector3Int(i,j,0)) is floorTileMap) {
                    // Vector2Int result = Vector2Int.zero;
                    // if ((TryGetTilePosition(out result, new Vector2Int(i, j) + Vector2Int.up) && grid[result.x, result.y] == tileSet.floor)
                    //     || (TryGetTilePosition(out result, new Vector2Int(i, j) + Vector2Int.down) && grid[result.x, result.y] == tileSet.floor)
                    //     || (TryGetTilePosition(out result, new Vector2Int(i, j) + Vector2Int.left) && grid[result.x, result.y] == tileSet.floor)
                    //     || (TryGetTilePosition(out result, new Vector2Int(i, j) + Vector2Int.right) && grid[result.x, result.y] == tileSet.floor)) {
                    //     grid[i, j] = tileSet.wall;
                    // }
                    wallTileMap.SetTile(new Vector3Int(i, j, 0), wallTile);
                }
            } 
        }
    }

    private void SpawnFloorMaker(Vector3Int pos)
    {
        FloorMaker maker = new FloorMaker();
        maker.Pos = pos;
        maker.Dir = RandomDirection();

        floorMakers.Add(maker);
    } 
}
