using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

// drunkard walk algorithm
public class LevelGenerator : MonoBehaviour {
    [SerializeField] private int iterations = 100;
    [SerializeField] private int chanceOf2x2Room = 50;
    [SerializeField] private int chanceOf3x3Room = 11;
    [SerializeField] private Vector2 levelSizeWorldUnits = new Vector2(30, 30);
    [SerializeField] private float worldUnitsPerGridCell = 1;

    enum tileSet {
        empty,
        floor,
        walll
    };
    private tileSet[,] grid;
    private int levelWidth, levelHeight;
    private List<FloorMaker> floorMakers;
    private Random random = new Random();

    struct FloorMaker {
        public Vector2 Dir;
        public Vector2 Pos;
    }

    private void Start() {
        Setup();
        GenerateFloors(iterations);
        GenerateWalls();
        SpawnLevel();
    }
    
    private Vector2 RandomDirection() {
        System.Random rand = new System.Random();
        int randInt = rand.Next(4);

        Vector2 direction = Vector2.zero;
        switch (randInt) {
            case 0: 
                direction = Vector2.up;
                break;
            case 1: 
                direction = Vector2.down;
                break;
            case 2: 
                direction = Vector2.left;
                break;
            case 3: 
                direction = Vector2.right;
                break;
            default: 
                Debug.LogWarningFormat("Invalid case");
                break;
        }
        return direction;
    }

    private void Setup() {
        // Determine grid size
        levelWidth = Mathf.RoundToInt(levelSizeWorldUnits.x / worldUnitsPerGridCell);
        levelHeight = Mathf.RoundToInt(levelSizeWorldUnits.y / worldUnitsPerGridCell);
        
        // Create grid
        grid = new tileSet[levelWidth, levelHeight];
        
        // Populate grid with empty tiles
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                grid[i, j] = tileSet.empty;
            } 
        }
        
        floorMakers = new List<FloorMaker>();
    }

    private void GenerateFloors(int iterations, int chance2x2, int chance3x3) {
        // Spawn an initial floorMaker
        FloorMaker initialMaker = new FloorMaker();
        initialMaker.Dir = RandomDirection();
        initialMaker.Pos = new Vector2(levelWidth / 2, levelHeight / 2);
        floorMakers.Add(initialMaker);
        
        
        int currentIteration = 0;
        do {
            for (int i = floorMakers.Count; i > 0; i--) {
                var floorMaker = floorMakers[i];
                
                // Spawn floor
                grid[(int)floorMaker.Pos.x, (int)floorMaker.Pos.y] = tileSet.floor;

                // Chance of ...
                //TODO: Improve code
                int rand = random.Next(0, 100);
                if (rand < chance2x2) { 
                    grid[(int)floorMaker.Pos.x+1, (int)floorMaker.Pos.y] = tileSet.floor;
                    grid[(int)floorMaker.Pos.x+1, (int)floorMaker.Pos.y+1] = tileSet.floor;
                    grid[(int)floorMaker.Pos.x, (int)floorMaker.Pos.y+1] = tileSet.floor;

                }
                if (rand < chance3x3) {
                    grid[(int)floorMaker.Pos.x+1, (int)floorMaker.Pos.y] = tileSet.floor;
                    grid[(int)floorMaker.Pos.x+1, (int)floorMaker.Pos.y+1] = tileSet.floor;
                    grid[(int)floorMaker.Pos.x, (int)floorMaker.Pos.y+1] = tileSet.floor;

                    grid[(int)floorMaker.Pos.x+2, (int)floorMaker.Pos.y] = tileSet.floor;
                    grid[(int)floorMaker.Pos.x+2, (int)floorMaker.Pos.y+1] = tileSet.floor;
                    grid[(int)floorMaker.Pos.x+2, (int)floorMaker.Pos.y+2] = tileSet.floor;
                    grid[(int)floorMaker.Pos.x+1, (int)floorMaker.Pos.y+2] = tileSet.floor;
                    grid[(int)floorMaker.Pos.x, (int)floorMaker.Pos.y+2] = tileSet.floor;
                }
                
                // Continue here
                
                // Move floorMaker
                floorMaker.Pos += floorMaker.Dir; //TODO: Validate whether inside grid or not 
                floorMaker.Dir = RandomDirection();
                floorMakers[i] = floorMaker;
            }
        } while (currentIteration++ < iterations);

    }
}
