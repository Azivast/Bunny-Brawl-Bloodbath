using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private float atttackRange;
    
    private WeaponBehaviour weaponBehaviour;
    private int bulletLayer = 11;

    private void Start() {
        weapon = Instantiate(weapon, weaponPosition);
        weaponBehaviour = weapon.GetComponent<WeaponBehaviour>();
        weaponBehaviour.InfiniteAmmo = true;
    }

    private bool TryFindTarget(out TargetBehaviour target) {
        int mask = LayerMask.GetMask("Player");
        Collider2D col = Physics2D.OverlapCircle(transform.position, atttackRange, mask);

        if (col == null) {
            target = null;
            return false;
        }
        else {
            target = col.GetComponent<TargetBehaviour>();
            return true;
        }
    }

    private void FixedUpdate() {
        if (TryFindTarget(out TargetBehaviour target)) {
            weaponBehaviour.Target = target.transform.position;
            weaponBehaviour.Shoot(bulletLayer);
        }
    }
}
