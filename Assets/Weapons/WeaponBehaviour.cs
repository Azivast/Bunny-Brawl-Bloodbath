using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponBehaviour : MonoBehaviour {
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform[] firingPositions;
    [SerializeField] private int ammo = 100;
    [SerializeField] private float fireRate = 1;
    
    public Vector3 target;
    private Quaternion rotation;
    private float fireTimer;

    private void Update() {
        Vector2 direction = (target - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.left, direction);

        if (fireTimer > 0) {
            fireTimer -= Time.deltaTime;
        }
    }

    public void Shoot() {
        if (!(fireTimer <= 0)) return;
        fireTimer = fireRate;
        foreach (Transform firingPosition in firingPositions) {
            if (ammo <= 0) return;
            Instantiate(projectile, firingPosition.position, firingPosition.rotation);
            ammo--;
        }
    }
}
