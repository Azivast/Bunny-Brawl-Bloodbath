using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Pickupable : MonoBehaviour
{
    public UnityEvent OnPickup;
    [Tooltip("Distance from pickup before it starts moving towards player")]
    [SerializeField] private float range = 1;
    [Tooltip("Speed at which the pickup moves towards the player when in range")]
    [SerializeField] private float speed = 4;


    private bool TryFindPlayer(out PlayerBehaviour player) {
        int mask = LayerMask.GetMask("Player");
        Collider2D col = Physics2D.OverlapCircle(transform.position, range, mask);

        if (col == null) {
            player = null;
            return false;
        }
        else {
            player = col.GetComponent<PlayerBehaviour>();
            return true;
        }
    }

    private void Update() {
        if (!TryFindPlayer(out PlayerBehaviour player)) return;

        Vector3 dir = (player.transform.position - transform.position).normalized;
        transform.position += dir * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            OnPickup.Invoke();
            Debug.Log("Picked up pickupable");
            Destroy(gameObject);
        }
    }
}
