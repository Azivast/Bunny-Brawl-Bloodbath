using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OneTimeDamage : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private int framesAlive = 1;

    private int currentFrame = 0;

    
    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.TryGetComponent(out TargetBehaviour target))
        {
            target.Attack(amount);

            if (other.gameObject.TryGetComponent(out HitParticle hitParticle))
            {
                Instantiate(hitParticle.Particle, other.transform.position, transform.rotation);
            }
        }

        Destroy(gameObject);
    }

    private void FixedUpdate() {
        if (currentFrame++ >= framesAlive) Destroy(gameObject);
    }
}
