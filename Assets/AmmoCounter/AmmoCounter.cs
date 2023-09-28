using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private float lowAmmoLimit = 0.2f;
    [SerializeField] private Color defaultAmmoColor = Color.white;
    [SerializeField] private Color lowAmmoColor = Color.red;

    private void OnEnable() {
        ammoType.OnAmountChancePercent += UpdateCounter;
        UpdateCounter((float)ammoType.GetAmmoLeft()/ammoType.GetMaxAmmo());
    }

    private void OnDisable() {
        ammoType.OnAmountChancePercent -= UpdateCounter;
    }

    private void UpdateCounter(float amount) {
        bar.fillAmount = amount;
        bar.color = amount < lowAmmoLimit ? lowAmmoColor : defaultAmmoColor;
    }
    
}
