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
    public class LevelChestSpawner
    {
        [SerializeField] private GameObject weaponChest;
        [SerializeField] private GameObject ammoChest;
        [SerializeField] private int maxNumberPerType = 5; 
        [SerializeField] private int minDistanceToSpawn = 1;
        [SerializeField] private Transform parent;
        [SerializeField] private List<Vector2> spawnedPositions;

        public void SpawnWeaponChests(List<Vector2> locations, Vector2 playerPos)
        {
            Spawn(weaponChest, locations, playerPos);
        }
        
        public void SpawnAmmoChests(List<Vector2> locations, Vector2 playerPos)
        {
            Spawn(ammoChest, locations, playerPos);
        }
        
        private void Spawn(GameObject chest, List<Vector2> locations, Vector2 playerPos)
        {
            List<Vector2> availableLocations = locations.Distinct().ToList(); // remove duplicate locations

            for (int i = 0; i < maxNumberPerType;)
            {
                if (availableLocations.Count <= 0) break;
                
                // Find suitable location that isn't too close to player or occupied
                Vector2 selectedLocation = availableLocations[ConstRandom.Random.Next(availableLocations.Count)];
                if ((selectedLocation - playerPos).magnitude < minDistanceToSpawn)
                {
                    availableLocations.Remove(selectedLocation);
                    continue;
                }
                if (spawnedPositions.Contains(selectedLocation))
                {
                    availableLocations.Remove(selectedLocation);
                    continue;
                }
                
                // Spawn
                Object.Instantiate(chest, (Vector2)selectedLocation, Quaternion.identity, parent);
                spawnedPositions.Add(selectedLocation);
                availableLocations.Remove(selectedLocation);
                i++;
            }
        }

        public void ClearObjects()
        {
            for (int i = parent.childCount-1; i >= 0; i--)
            {
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
            }
            spawnedPositions.Clear();
        }
    }
}