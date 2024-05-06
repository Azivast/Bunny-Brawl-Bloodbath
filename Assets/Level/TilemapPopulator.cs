using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using LevelGenerator = ProceduralGeneration.LevelGenerator;

namespace ProceduralGeneration
{
    [Serializable]
    public class TilemapPopulator
    {
        [SerializeField] private Tilemap groundMap, wallMap;
        [SerializeField] private Tile groundTile, wallsTile;

        public void Populate(LevelGenerator.AvailableTiles[,] levelData)
        {
            // Reset
            groundMap.ClearAllTiles();
            wallMap.ClearAllTiles();
            
            // Populate
            for (var y = 0; y < levelData.GetLength(1); y++) // loop through y
            {
                for (var x = 0; x < levelData.GetLength(0); x++) // loop through x
                {
                    var tile = levelData[x, y];
                    if (tile == LevelGenerator.AvailableTiles.Ground)
                    {
                        groundMap.SetTile(new Vector3Int(x, y), groundTile);
                    }
                }
            }
        }
    }
}
