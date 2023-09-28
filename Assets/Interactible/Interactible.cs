using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Interactible : MonoBehaviour
{
    public UnityEvent OnInteract;
    public string InteractText = "interact";
    [SerializeField] private InteractHandlerObject interactHandler;
    private bool inRange;

    private void OnEnable() {
        interactHandler.OnPlayerInteract += Interact;
    }
    private void OnDisable() {
        interactHandler.OnPlayerInteract -= Interact;
    }

    private void Interact() {
        if (inRange) {
            OnInteract.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        inRange = true;
    }
    
    private void OnTriggerExit2D(Collider2D col) {
        inRange = false;
    }
}

