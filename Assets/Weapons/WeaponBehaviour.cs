using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponBehaviour : MonoBehaviour {
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform[] firingPositions;
    [SerializeField] private float fireRate = 1;
    
    private Quaternion rotation;
    private float fireTimer;
    private SpriteRenderer spriteRenderer;

    public Vector3 Target { get; set; }

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        Vector2 direction = (Target - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
        if (transform.rotation.z > -Mathf.Deg2Rad*45 && transform.rotation.z < Mathf.Deg2Rad*45) {
            spriteRenderer.flipY = false;

        }
        else {
            spriteRenderer.flipY = true;
        }

        if (fireTimer > 0) {
            fireTimer -= Time.deltaTime;
        }
    }

    public void Shoot() {
        if (!(fireTimer <= 0)) return;
        fireTimer = fireRate;
        foreach (Transform firingPosition in firingPositions) {
            Instantiate(projectile, firingPosition.position, firingPosition.rotation);
        }
    }
}
