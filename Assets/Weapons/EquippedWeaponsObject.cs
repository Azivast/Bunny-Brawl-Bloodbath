using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "EquippedWeapons", menuName = "Bunny Brawl Bloodbath/EquippedWeaponsObject")]
public class EquippedWeaponsObject : ScriptableObject
{
    [SerializeField] private GameObject[] startingWeapons = new GameObject[2];
    [SerializeField] private GameObject[] weapons = new GameObject[2];

    public int ActiveWeaponIndex = 0;
    public UnityAction<GameObject> OnWeaponEquipped;
    public GameObject[] List => weapons;

    public void Reset() {
        weapons = new GameObject[startingWeapons.Length];
        for (int i = 0; i < startingWeapons.Length; i++) {
            weapons[i] = Instantiate(startingWeapons[i]);
            weapons[i].SetActive(false);
        }
    }

    private void OnDisable() {
        for (int i = startingWeapons.Length-1; i >= 0; i--) {
#if UNITY_EDITOR
            DestroyImmediate(weapons[i]);
#else
        Destroy(weapons[i]);
#endif
        }
    }

    public void EquipNewWeapon(GameObject weapon) {
        weapon.SetActive(false);
        weapons[ActiveWeaponIndex] = weapon;
        OnWeaponEquipped.Invoke(weapons[ActiveWeaponIndex]);
    }

    public void AddAmmoRandomWeapon(int amount) {
        System.Random rand = new System.Random();
        int i = rand.Next(0, weapons.Length);
        
        weapons[i].GetComponent<WeaponBehaviour>().AmmoType.AddAmmo(amount);
    }
}
