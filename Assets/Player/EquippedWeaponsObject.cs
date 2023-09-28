using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EquippedWeapons", menuName = "Bunny Brawl Bloodbath/EquippedWeaponsObject")]
public class EquippedWeaponsObject : ScriptableObject
{
    [SerializeField] private GameObject[] weapons = new GameObject[2];

    public GameObject[] List => weapons;

    public void EquipNewWeapon(int index, GameObject weapon) {
        weapons[index] = weapon;
    }

    public void AddAmmoRandomWeapon(int amount) {
        System.Random rand = new System.Random();
        int i = rand.Next(0, weapons.Length);
        
        weapons[i].GetComponent<WeaponBehaviour>().AmmoType.AddAmmo(amount);
    }
}
