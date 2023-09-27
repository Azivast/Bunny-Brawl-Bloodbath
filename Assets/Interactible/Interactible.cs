using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Interactible : MonoBehaviour
{
    public UnityEvent OnInteract;
    [SerializeField] private InteractHandlerObject interactHandler;

    private void OnEnable() {
        interactHandler.OnPlayerInteract += Interact;
    }
    private void OnDisable() {
        interactHandler.OnPlayerInteract -= Interact;
    }

    private void Interact(Interactible interactable) {
        if (interactable == this) OnInteract.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        interactHandler.InRange(this);
    }
    
    private void OnTriggerExit2D(Collider2D col) {
        interactHandler.LeftRange(this);
    }
}

