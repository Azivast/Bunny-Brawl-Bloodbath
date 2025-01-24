using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {
    [SerializeField] private int speed = 5;
    [SerializeField] private int damage = 1;
    [SerializeField] private int bounces = 0;
    [SerializeField] private GameObject defaultHitParticle;

    private Rigidbody2D rb;
    private int bouncesLeft;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        bouncesLeft = bounces;
    }

    private void FixedUpdate() {
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        if (col.gameObject.TryGetComponent(out TargetBehaviour target))
        {
            target.Attack(damage);

            if (col.gameObject.TryGetComponent(out HitParticle hitParticle))
            {
                Instantiate(hitParticle.Particle, col.contacts[0].point, transform.rotation);
            }
            else
            {
                Instantiate(defaultHitParticle, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal));
            }
        }

        else if (col.gameObject.TryGetComponent(out HitParticle hitParticle))
        {
            Instantiate(hitParticle.Particle, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal));
        }
        else
        {
            Instantiate(defaultHitParticle, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal));
        }


        if (bouncesLeft <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.forward = Vector2.Reflect(transform.forward, col.contacts[0].normal);
            bouncesLeft--;
            rb.velocity = transform.forward * speed;
        }

    }
}
