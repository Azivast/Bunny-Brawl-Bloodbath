using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour {
    [SerializeField] private EquippedWeaponsObject weapons;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private CameraController cam;
    [SerializeField] private InputActionReference fire, switchWeapon;
    [SerializeField] private PopupSpawner popupSpawner;
    
    private GameObject activeWeapon;
    private WeaponBehaviour activeWeaponBehaviour;
    private int bulletLayer = 8;

    private void Start() {
        ChangeToWeapon(0);
    }

    private void OnEnable() {
        fire.action.Enable();
        switchWeapon.action.Enable();
        fire.action.performed += OnFire;
        switchWeapon.action.performed += CycleWeapon;
        weapons.OnWeaponEquipped += OnWeaponEquipped;
    }
    
    private void OnDisable() {
        fire.action.Disable();
        fire.action.performed -= OnFire;
        switchWeapon.action.Disable();
        switchWeapon.action.performed -= CycleWeapon;
        weapons.OnWeaponEquipped -= OnWeaponEquipped;
        
        for (int i = 0; i < weaponPosition.childCount; i++) {
            Destroy(weaponPosition.GetChild(i));
        }
    }

    private void Aim() {
        activeWeaponBehaviour.Target = cam.MouseWorldPosition();
    }

    private void Update() {
        Aim();
        if (fire.action.IsPressed()) activeWeaponBehaviour.Shoot(bulletLayer);
    }

    private void ChangeToWeapon(int index) {
        for (int i = 0; i < weaponPosition.childCount; i++) {
            weaponPosition.GetChild(i).gameObject.SetActive(false);
        }

        activeWeapon = weapons.List[index];
        activeWeapon.transform.parent = weaponPosition;
        activeWeapon.transform.localPosition = Vector3.zero;
        activeWeaponBehaviour = activeWeapon.GetComponent<WeaponBehaviour>();
        activeWeapon.SetActive(true);
    }

    private void OnWeaponEquipped(GameObject gameObject) {
        activeWeapon.transform.parent = null; // Drop active weapon
        activeWeapon.transform.rotation = Quaternion.identity;
        ChangeToWeapon(weapons.ActiveWeaponIndex);
        popupSpawner.SpawnText($"picked up {gameObject.GetComponent<WeaponBehaviour>().WeaponName}");
    }

    private void OnFire(InputAction.CallbackContext context) {
        //if (context.ReadValueAsButton()) activeWeaponBehaviour.Shoot(bulletLayer); // todo: cleanup
    }
    
    private void CycleWeapon(InputAction.CallbackContext context) {
        weapons.ActiveWeaponIndex++;
        if (weapons.ActiveWeaponIndex > weapons.List.Length - 1) weapons.ActiveWeaponIndex = 0;
        
        ChangeToWeapon(weapons.ActiveWeaponIndex);
    }
}
