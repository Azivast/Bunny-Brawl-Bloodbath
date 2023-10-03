using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AmmoType", menuName = "Bunny Brawl Bloodbath/AmmoType")]
public class AmmoType : ScriptableObject
{
    public UnityAction<int> OnAmountChance = delegate{};
    public UnityAction<float> OnAmountChancePercent = delegate{};

    [SerializeField] private int maxAmmo = 1;
    [SerializeField] private int startingAmmo;
    [SerializeField] private int ammoLeft;

    public void Reset() {
        ammoLeft = startingAmmo;
        OnAmountChancePercent((float)ammoLeft / maxAmmo);
    }

    public int GetAmmoLeft() {
        return ammoLeft;
    }
    
    public int GetMaxAmmo() {
        return maxAmmo;
    }
    

    public void AddAmmo(int amount) {
        ammoLeft = Math.Min(ammoLeft + amount, maxAmmo);
        OnAmountChance(amount);
        OnAmountChancePercent((float)ammoLeft / maxAmmo);
    }
    
    public void UseAmmo(int amount) {
        ammoLeft = Math.Max(ammoLeft - amount, 0);
        OnAmountChance(amount);
        OnAmountChancePercent((float)ammoLeft / maxAmmo);
    }
}
