using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace ProceduralGeneration
{
    [Serializable]
    public class LevelEnemySpawner
    {
        [SerializeField] private List<GameObject> enemies;
        [SerializeField] private int maxEnemies = 20; 
        [SerializeField] private double spawnChance = 1; 
        [SerializeField] private int minDistanceToSpawn = 5;
        [SerializeField] private Transform parent;
        private int enemiesSpawned;

        public void SpawnEnemies(LevelGenerator.AvailableTiles[,] levelData, Vector2 playerSpawn, Vector2 offset)
        {
            int attempts = 0; // Number of failed spawn attempts before function returns 
            const int maxAttempts = 1000;
            
            while (enemiesSpawned < maxEnemies && attempts < maxAttempts)
            {
                bool enemySpawnedThisIteration = false;
                
                for (var y = 0; y < levelData.GetLength(1); y++) // loop through y
                {
                    for (var x = 0; x < levelData.GetLength(0); x++) // loop through x
                    {
                        if ((playerSpawn - new Vector2(x, y)).magnitude < minDistanceToSpawn)
                        {
                            continue;
                        }
                    
                        var tile = levelData[x, y];
                        if (tile == LevelGenerator.AvailableTiles.Ground)
                        {
                            double chance = UnityEngine.Random.Range(0,100)*100; // NextDouble returns [0,1]*100=[0,100]
                            if (chance < spawnChance)
                            {
                                Object.Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Count)], new Vector3(x, y) + (Vector3)offset, Quaternion.identity, parent);
                                enemiesSpawned++;
                                enemySpawnedThisIteration = true;
                                
                                if (enemiesSpawned >= maxEnemies) return;
                            }
                        }
                    }
                }

                if (enemySpawnedThisIteration == false) attempts++;
                else attempts = 0;
            }
        }

        public void ClearEnemies()
        {
            for (int i = parent.childCount-1; i >= 0; i--)
            {
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
            }

            enemiesSpawned = 0;
        }
    }
}