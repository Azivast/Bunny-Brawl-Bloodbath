using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform weaponPosition;
  

    private WeaponBehaviour activeWeaponBehaviour;

    private void Start() {
        weapon = Instantiate(weapon, weaponPosition);
        activeWeaponBehaviour = weapon.GetComponent<WeaponBehaviour>();
    }


    private void Aim() {
        activeWeaponBehaviour.target = mouseWorldPosition;
    }
}
