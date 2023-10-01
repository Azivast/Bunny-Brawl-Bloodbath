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
}
