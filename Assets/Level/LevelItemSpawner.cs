using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace ProceduralGeneration
{
    [Serializable]
    public class LevelItemSpawner
    {
        [SerializeField] private int maxNumberOfItems = 5; 
        [SerializeField] private int minDistanceToSpawn = 1; 
        public ObjectCollection SpawnableItems;
        
        public void SpawnItems(List<Vector2> potentialLocations, Vector2 playerPos, Random random, Transform parent)
        {
            List<Vector2> availableLocations = new List<Vector2>(potentialLocations);

            for (int i = 0; i < maxNumberOfItems; i++)
            {
                if (availableLocations.Count <= 0) break;
                Vector2 selectedLocation = availableLocations[random.Next(availableLocations.Count())];
                while ((selectedLocation - playerPos).magnitude < minDistanceToSpawn) // make sure we're not picking position too close to the player spawn
                {
                    availableLocations.Remove(selectedLocation);
                    selectedLocation = potentialLocations[random.Next(potentialLocations.Count())];
                }

                int selectedObject = random.Next(SpawnableItems.GetObjects().Count());
                Object.Instantiate(SpawnableItems.GetObjects()[selectedObject], (Vector2)selectedLocation, Quaternion.identity, parent);
            }
        }
    }
}