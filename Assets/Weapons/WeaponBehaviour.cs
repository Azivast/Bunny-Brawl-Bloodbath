using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponBehaviour : MonoBehaviour {
    public string WeaponName;
    [SerializeField] private GameObject projectile;
    public AmmoType AmmoType;
    [SerializeField] private Transform[] firingPositions;
    [SerializeField] private float fireRate = 1;
    [SerializeField] private UnityEvent onFire;
    [SerializeField] private UnityEvent onPostFire;
    [SerializeField] private float postFireDelay = 0.1f;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeIntensity = 0.05f;
    public bool InfiniteAmmo = false;
    
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

    public void Shoot(int projectileLayer) {
        if (!(fireTimer <= 0)) return;

        fireTimer = fireRate;
        foreach (Transform firingPosition in firingPositions) {
            if (InfiniteAmmo == false) {
                if (AmmoType.GetAmmoLeft() < 1) return;
                else AmmoType.UseAmmo(1);
            }
            var bullet = Instantiate(projectile, firingPosition.position, firingPosition.rotation);
            bullet.layer = projectileLayer;
        }
        onFire.Invoke();
        CameraShake.Shake(shakeDuration, shakeIntensity);
        StartCoroutine(PostShoot());
    }
    
    IEnumerator PostShoot()
    {
        yield return new WaitForSeconds(postFireDelay);
        onPostFire.Invoke();
    }
}
