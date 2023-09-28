using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "InteractHandler", menuName = "Bunny Brawl Bloodbath/InteractHandler")]
public class InteractHandlerObject : ScriptableObject {
    public UnityAction OnPlayerInteract;

    public void Interact() {
        OnPlayerInteract.Invoke();
    }
    
    
    //TODO: Handle interacting with 2 at the same time
    //
    // public void Update(Vector3 playerPosition) {
    //     if (inRange.Count <= 1) return;
    //     
    //     var newClosest = inRange[0];
    //     for (int i = 1; i < inRange.Count; i++) {
    //         if ((inRange[i].transform.position - playerPosition).magnitude < (inRange[i-1].transform.position - playerPosition).magnitude) {
    //             newClosest = inRange[i];
    //         }
    //     }
    //     if (newClosest != closest) {
    //         closest = newClosest;
    //         OnNewClosestInRange.Invoke();
    //     }
    // }
    //
    // public void InRange(Interactible interactable) {
    //     inRange.Add(interactable);
    // }
    //
    // public void LeftRange(Interactible interactable) {
    //     if (inRange.Any()) OnNoneInRange.Invoke();
    //     
    //     inRange.Remove(interactable);
    // }
}
