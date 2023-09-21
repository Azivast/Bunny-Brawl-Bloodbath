using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour {
    //[SerializeField] private GameObject[] weapons;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private GameObject activeWeapon;
    [SerializeField] private Camera camera;
    [SerializeField] private InputActionReference fire, look;
    
    private WeaponBehaviour activeWeaponBehaviour;

    private void Start() {
        activeWeapon = Instantiate(activeWeapon, weaponPosition);
        activeWeaponBehaviour = activeWeapon.GetComponent<WeaponBehaviour>();
    }

    private void OnEnable() {
        fire.action.Enable(); // TODO: is this needed?
        look.action.Enable();
        fire.action.performed += Fire;
    }
    
    private void OnDisable() {
        fire.action.Disable(); // TODO: is this needed?
        look.action.Disable();
        fire.action.performed -= Fire;
    }

    private void Aim() {
        var mousePosition = look.action.ReadValue<Vector2>();
        var mousePositionZ = camera.farClipPlane * .5f;

        var mouseWorldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mousePositionZ));

        activeWeaponBehaviour.target = mouseWorldPosition;
    }

    private void Update() {
        Aim();
    }

    private void ChangeWeapon(int index) {
        // for (int i = 0; i < weapons.Length; i++) {
        //     weapons[i].SetActive(false);
        // }
    }

    private void Fire(InputAction.CallbackContext context) {
        activeWeaponBehaviour.Shoot(); // TODO: Use events and reduce coupling!
    }
}
