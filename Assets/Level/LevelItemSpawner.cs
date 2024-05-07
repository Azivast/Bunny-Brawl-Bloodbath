using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace ProceduralGeneration
{
    [Serializable]
    public class LevelItemSpawner
    {
        [SerializeField] private int maxNumberOfItems = 5; 
        [SerializeField] private int minDistanceToSpawn = 1; 
        public ObjectCollection SpawnableItems;

        public void SpawnItems(List<Vector2Int> potentialLocations, Vector2Int playerPos, Random random)
        {
            List<Vector2Int> availableLocations = new List<Vector2Int>(potentialLocations); // deep copy (todo: verify this works as i expect)

            for (int i = 0; i < maxNumberOfItems;)
            {
                Vector2Int selectedLocation = availableLocations[random.Next(availableLocations.Count())];
                while ((selectedLocation - playerPos).magnitude < minDistanceToSpawn) // make sure we're not picking position too close to the player spawn
                {
                    availableLocations.Remove(selectedLocation);
                    selectedLocation = potentialLocations[random.Next(potentialLocations.Count())];
                }

                int selectedObject = random.Next(SpawnableItems.GetObjects().Count());
                MonoBehaviour.Instantiate(SpawnableItems.GetObjects()[selectedObject], (Vector2)selectedLocation, Quaternion.identity);
            }
        }
    }
}