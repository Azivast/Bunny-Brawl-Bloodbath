using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour {
    //[SerializeField] private GameObject[] weapons;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private GameObject activeWeapon;
    [SerializeField] private InputActionReference fire, look;

    private void Start() {
        activeWeapon = Instantiate(activeWeapon, weaponPosition);
    }

    private void OnEnable() {
        fire.action.Enable(); // TODO: is this needed?
        look.action.Enable();
        fire.action.performed += Fire;
        look.action.performed += Aim;
    }
    
    private void OnDisable() {
        fire.action.Disable(); // TODO: is this needed?
        look.action.Disable();
        fire.action.performed -= Fire;
        look.action.performed -= Aim;
    }

    private void Aim(InputAction.CallbackContext context) {
        activeWeapon.GetComponent<WeaponBehaviour>().target = context.ReadValue<Vector2>();
    }
    
    private void ChangeWeapon(int index) {
        // for (int i = 0; i < weapons.Length; i++) {
        //     weapons[i].SetActive(false);
        // }
    }

    private void Fire(InputAction.CallbackContext context) {
        activeWeapon.GetComponent<WeaponBehaviour>().Shoot(); // TODO: Use events and reduce coupling!
    }
}
