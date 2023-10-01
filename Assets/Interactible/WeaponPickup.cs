using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour
{
    public UnityAction OnInteract;
    [SerializeField] private InteractHandlerObject interactHandler;
    [SerializeField] private EquippedWeaponsObject equipedWeapons;
    [SerializeField] private UnityEvent<bool> OnInRange;
    private bool inRange;

    private void OnEnable() {
        interactHandler.OnPlayerInteract += Interact;
    }
    private void OnDisable() {
        interactHandler.OnPlayerInteract -= Interact;
    }

    private void Interact() {
        if (inRange && transform.parent == null) {
            equipedWeapons.EquipNewWeapon(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        inRange = true;
        OnInRange.Invoke(true);
    }
    
    private void OnTriggerExit2D(Collider2D col) {
        inRange = false;
        inRange = false;
        OnInRange.Invoke(false);
    }
}

