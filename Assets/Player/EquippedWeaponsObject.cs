using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquippedWeapons", menuName = "EquippedWeaponsObject")]
public class EquippedWeaponsObject : ScriptableObject
{
    [SerializeField] private GameObject[] weapons = new GameObject[2];
    private int activeWeaponIndex = 0;
    private WeaponBehaviour activeWeaponBehaviour;

    private void Awake() {
        activeWeaponBehaviour = weapons[activeWeaponIndex].GetComponent<WeaponBehaviour>();
    }

    public GameObject GetActiveWeapon() {
        return weapons[activeWeaponIndex];
    }
    
    public WeaponBehaviour GetActiveWeaponBehavior() {
        return activeWeaponBehaviour;
    }
    
    public void SwitchWeapon() {
        activeWeaponIndex++;
        if (activeWeaponIndex > weapons.Length-1) activeWeaponIndex = 0;
        activeWeaponBehaviour = weapons[activeWeaponIndex].GetComponent<WeaponBehaviour>();
    }
    
    public void EquipNewWeapon(GameObject newWeapon) {
        weapons[activeWeaponIndex] = newWeapon;
    }

    public void AddAmmoRandomWeapon(int amount) {
        System.Random rand = new System.Random();
        int i = rand.Next(0, weapons.Length);
        
        weapons[i].GetComponent<WeaponBehaviour>().AmmoType.AddAmmo(amount);
    }
}
