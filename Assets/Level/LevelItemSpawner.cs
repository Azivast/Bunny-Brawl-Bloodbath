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
        [SerializeField] private GameObject weaponChest;
        [SerializeField] private GameObject ammoChest;
        [SerializeField] private int maxNumberPerType = 5; 
        [SerializeField] private int minDistanceToSpawn = 1;
        [SerializeField] private Transform parent;
        [SerializeField] private List<Vector2> spawnedPositions;

        public void SpawnWeaponChests(List<Vector2> locations, Vector2 playerPos)
        {
            SpawnItems(weaponChest, locations, playerPos);
        }
        
        public void SpawnAmmoChests(List<Vector2> locations, Vector2 playerPos)
        {
            SpawnItems(ammoChest, locations, playerPos);
        }
        
        private void SpawnItems(GameObject item, List<Vector2> locations, Vector2 playerPos)
        {
            List<Vector2> availableLocations = new List<Vector2>(locations);

            for (int i = 0; i < maxNumberPerType; i++)
            {
                if (availableLocations.Count <= 0) break;
                Vector2 selectedLocation = availableLocations[ConstRandom.Random.Next(availableLocations.Count())];
                while ((selectedLocation - playerPos).magnitude < minDistanceToSpawn) // make sure we're not picking position too close to the player spawn
                {
                    availableLocations.Remove(selectedLocation);
                    selectedLocation = locations[ConstRandom.Random.Next(locations.Count())];
                }
                Object.Instantiate(item, (Vector2)selectedLocation, Quaternion.identity, parent);
                spawnedPositions.Add(selectedLocation);
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