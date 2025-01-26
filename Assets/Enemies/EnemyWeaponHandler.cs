using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private float attackRange = 3;
    [SerializeField] private float shootTimeBeforeCooldown = 1;
    [SerializeField] private float cooldownLength = 2.5f;
    [SerializeField] private bool doCooldown = true;
    [SerializeField] LayerMask raycastHitLayers;
    
    private WeaponBehaviour weaponBehaviour;
    private int bulletLayer = 11;
    private float cooldown;
    private bool coolingdown= false;
    private float shootTime = 0;

    private void Start() {
        weapon = Instantiate(weapon, weaponPosition);
        weaponBehaviour = weapon.GetComponent<WeaponBehaviour>();
        weaponBehaviour.InfiniteAmmo = true;
        cooldown = 0;
    }

    private bool TryFindTarget(out TargetBehaviour target) {
        int mask = LayerMask.GetMask("Player");
        Collider2D col = Physics2D.OverlapCircle(transform.position, attackRange, mask);

        if (col !=null && col.TryGetComponent<TargetBehaviour>(out var playerTarget))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (playerTarget.transform.position-transform.position).normalized, attackRange, raycastHitLayers);
            if (hit == true && hit.transform.TryGetComponent<PlayerBehaviour>(out _))
            { 
                Debug.DrawRay(transform.position, (playerTarget.transform.position-transform.position).normalized * attackRange, Color.red); 
                target = playerTarget;
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, (playerTarget.transform.position-transform.position).normalized * attackRange, Color.yellow); 
            }
        }

        target = null;
        return false;
    }

    private void FixedUpdate() 
    {
        if (TryFindTarget(out TargetBehaviour target)) 
        {
            weaponBehaviour.Target = target.transform.position;
            if (coolingdown == false)
            {
                weaponBehaviour.Target = target.transform.position + UnityEngine.Random.insideUnitSphere;// * (weaponBehaviour.Target-target.transform.position).magnitude;
                
                weaponBehaviour.Shoot(bulletLayer);
                shootTime += Time.deltaTime;

                if (shootTime >= shootTimeBeforeCooldown)
                {
                    coolingdown = true;
                    cooldown = cooldownLength;
                }
            }
            else
            {
                cooldown -= Time.deltaTime;
                if (cooldown <= 0)
                {
                    coolingdown = false;
                    shootTime = 0;
                }
            }
        }
    }
}
