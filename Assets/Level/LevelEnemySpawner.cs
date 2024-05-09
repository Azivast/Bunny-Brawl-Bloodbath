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
        [SerializeField] private Vector3 tileMiddleOffset = new Vector3(0.5f, 0.5f);
        private int enemiesSpawned;

        public void SpawnEnemies(LevelGenerator.AvailableTiles[,] levelData, Vector2 playerSpawn)
        {
            while (enemiesSpawned < maxEnemies)
            {
                for (var y = 0; y < levelData.GetLength(1); y++) // loop through y
                {
                    for (var x = 0; x < levelData.GetLength(0); x++) // loop through x
                    {
                        if ((playerSpawn - new Vector2(x, y)).magnitude < minDistanceToSpawn) break;
                    
                        var tile = levelData[x, y];
                        if (tile == LevelGenerator.AvailableTiles.Ground)
                        {
                            double chance = ConstRandom.Random.NextDouble()*100; // NextDouble returns [0,1]*100=[0,100]
                            if (chance < spawnChance)
                            {
                                Object.Instantiate(enemies[ConstRandom.Random.Next(enemies.Count)], new Vector3(x, y) + tileMiddleOffset, Quaternion.identity, parent);
                                enemiesSpawned++;
                                if (enemiesSpawned >= maxEnemies) return;
                            }
                        }
                    }
                }
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