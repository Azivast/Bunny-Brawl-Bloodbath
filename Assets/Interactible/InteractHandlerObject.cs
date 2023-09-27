using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "InteractHandler", menuName = "Bunny Brawl Bloodbath/InteractHandler")]
public class InteractHandlerObject : ScriptableObject {
    public UnityAction<Interactible> OnPlayerInteract;
    public UnityAction<GameObject> OnNewClosestInRange;
    public UnityAction OnNoneInRange;
    private List<Interactible> inRange = new();
    private Interactible closest;
    
    public void InteractWithClosest(Vector3 playerPosition) {
        if (!inRange.Any()) return;

        OnPlayerInteract.Invoke(closest);
    }

    public void Update(Vector3 playerPosition) {
        if (inRange.Count <= 1) return;
        
        closest = inRange[0];
        for (int i = 1; i < inRange.Count; i++) {
            if ((inRange[i].transform.position - playerPosition).magnitude < (inRange[i-1].transform.position - playerPosition).magnitude) {
                closest = inRange[i];
            }
        }
    }

    public void InRange(Interactible interactable) {
        if (!inRange.Any()) OnNewClosestInRange.Invoke(interactable.gameObject);

        inRange.Add(interactable);
    }
    
    public void LeftRange(Interactible interactable) {
        if (inRange.Any()) OnNoneInRange.Invoke();
        
        inRange.Remove(interactable);
    }
}
