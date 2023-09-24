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
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private int seed = 1234567;
    [SerializeField] private int iterations = 10;
    [SerializeField] private float chanceOf2x2Room = 50;
    [SerializeField] private float chanceOf3x3Room = 11;
    [SerializeField] private float chanceOfNewFloorMaker = 1;
    [SerializeField] private float chanceOfFloorMakerDeath = 1;
    [SerializeField] private float chanceOfFloorMakerDeathIncrease = 1;
    [SerializeField] private int levelWidth, levelHeight;

    enum tileSet {
        empty,
        floor,
        wall
    }
    private tileSet[,] grid;

    private List<FloorMaker> floorMakers;
    private Random random;
    private float chanceOfFloorMakerDeathInternal;

    struct FloorMaker {
        public Vector2Int Dir;
        public Vector2Int Pos;
    }

    private void OnValidate() {
        //TODO: validate
    }

    private void Start() {
        random = new Random(seed);
        Setup();
        GenerateFloors(iterations, chanceOf2x2Room, chanceOf3x3Room);
        GenerateWalls();
        SpawnLevel();
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

    private bool TryGetFloorPosition(out Vector2Int newTilePosition, Vector2Int currentTilePos) {
        // If requested tile is outside grid
        if (   
            currentTilePos.x > levelWidth-1 // -1 is padding in order to allow walls on outer edge of grid
            || currentTilePos.x < 1
            || currentTilePos.y > levelHeight-1
            || currentTilePos.y < 1   
        ) {
            newTilePosition = currentTilePos;
            return false;
        }

        newTilePosition = currentTilePos;
        return true;
    }
    private bool TryGetTilePosition(out Vector2Int newTilePosition, Vector2Int currentTilePos) {
        // If requested tile is outside grid
        if (   
            currentTilePos.x > levelWidth // -1 is padding in order to allow walls on outer edge of grid
            || currentTilePos.x < 0
            || currentTilePos.y > levelHeight
            || currentTilePos.y < 0
        ) {
            newTilePosition = currentTilePos;
            return false;
        }

        newTilePosition = currentTilePos;
        return true;
    }

    private void Setup() {
        // Create grid
        grid = new tileSet[levelWidth, levelHeight];

        // Populate grid with empty tiles
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                grid[i, j] = tileSet.empty;
            } 
        }

        chanceOfFloorMakerDeathInternal = chanceOfFloorMakerDeath;
        floorMakers = new List<FloorMaker>();
    }

    private void GenerateFloors(int iterations, float chance2x2, float chance3x3) {
        // Spawn an initial floorMaker
        SpawnFloorMaker(new Vector2Int(levelWidth / 2, levelHeight / 2));

        int currentIteration = 0;
        do {
            for (int i = floorMakers.Count-1; i >= 0; i--) {
                var floorMaker = floorMakers[i];
                Vector2Int newPos = new Vector2Int();
                int rand;
                
                // STEP 1: Spawn floor
                if (TryGetFloorPosition(out newPos, floorMaker.Pos)) {
                    grid[newPos.x, newPos.y] = tileSet.floor;
                }


                // STEP 2: Make rooms
                //TODO: Improve code
                rand = random.Next(0, 100);
                // if (rand < chance2x2) { 
                //     grid[floorMaker.Pos.x+1, floorMaker.Pos.y] = tileSet.floor;
                //     grid[floorMaker.Pos.x+1, floorMaker.Pos.y+1] = tileSet.floor;
                //     grid[floorMaker.Pos.x, floorMaker.Pos.y+1] = tileSet.floor;
                //     
                //     if (TryGetNeighbour(out newPos, floorMaker.Pos))
                // }
                // if (rand < chance3x3) {
                //     grid[floorMaker.Pos.x+1, floorMaker.Pos.y] = tileSet.floor;
                //     grid[floorMaker.Pos.x+1, floorMaker.Pos.y+1] = tileSet.floor;
                //     grid[floorMaker.Pos.x, floorMaker.Pos.y+1] = tileSet.floor;
                //
                //     grid[floorMaker.Pos.x+2, floorMaker.Pos.y] = tileSet.floor;
                //     grid[floorMaker.Pos.x+2, floorMaker.Pos.y+1] = tileSet.floor;
                //     grid[floorMaker.Pos.x+2, floorMaker.Pos.y+2] = tileSet.floor;
                //     grid[floorMaker.Pos.x+1, floorMaker.Pos.y+2] = tileSet.floor;
                //     grid[floorMaker.Pos.x, floorMaker.Pos.y+2] = tileSet.floor;
                // }
                
                // STEP 3: Split corridors
                rand = random.Next(0, 100);
                if (rand < chanceOfNewFloorMaker)
                {
                    SpawnFloorMaker(floorMaker.Pos);
                    chanceOfFloorMakerDeathInternal += chanceOfFloorMakerDeathIncrease;
                }
                
                // Move floorMaker
                if (TryGetFloorPosition(out _, floorMaker.Pos + floorMaker.Dir)) {
                    floorMaker.Pos += floorMaker.Dir;
                    floorMaker.Dir = RandomDirection();
                    floorMakers[i] = floorMaker;
                }
                
                // chance of death
                rand = random.Next(0, 100);
                if (floorMakers.Count > 1 && rand < chanceOfFloorMakerDeathInternal)
                {
                    floorMakers.Remove(floorMaker);
                }
            } 
        } while (currentIteration++ < iterations);
    }

    private void GenerateWalls() { //TODO: Optimize
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (grid[i, j] == tileSet.empty) {
                    // Vector2Int result = Vector2Int.zero;
                    // if ((TryGetTilePosition(out result, new Vector2Int(i, j) + Vector2Int.up) && grid[result.x, result.y] == tileSet.floor)
                    //     || (TryGetTilePosition(out result, new Vector2Int(i, j) + Vector2Int.down) && grid[result.x, result.y] == tileSet.floor)
                    //     || (TryGetTilePosition(out result, new Vector2Int(i, j) + Vector2Int.left) && grid[result.x, result.y] == tileSet.floor)
                    //     || (TryGetTilePosition(out result, new Vector2Int(i, j) + Vector2Int.right) && grid[result.x, result.y] == tileSet.floor)) {
                    //     grid[i, j] = tileSet.wall;
                    // }
                    grid[i, j] = tileSet.wall;
                }
            } 
        }
    }

    private void SpawnLevel() {
        floorTileMap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
        //tileMap.size = new Vector3Int(levelWidth, levelHeight); //TODO: Does this not work?? Lag? needed?

        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (grid[i, j] is tileSet.floor) {
                    floorTileMap.SetTile(new Vector3Int(i, j, 0), floorTile);
                }
                else if (grid[i, j] is tileSet.wall) {
                    wallTileMap.SetTile(new Vector3Int(i, j, 0), wallTile);
                }
            } 
        }
    }
    
    
    private void SpawnFloorMaker(Vector2Int pos)
    {
        FloorMaker maker = new FloorMaker();
        maker.Pos = pos;
        maker.Dir = RandomDirection();

        floorMakers.Add(maker);
    } 
}
