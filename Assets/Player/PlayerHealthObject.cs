using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "PlayerHealth", menuName = "Bunny Brawl Bloodbath/PlayerHealthObject")]
public class PlayerHealthObject : ScriptableObject
{
    public int MaxHealth = 3;
    public int CurrentHealth;
    public UnityAction<int, int> OnHealthChange = delegate{}; // <newHealth, maxHealth>
    public UnityAction OnPlayerDied = delegate { };

    public void Reset() {
        CurrentHealth = MaxHealth;
        OnHealthChange(CurrentHealth, MaxHealth);
    }

    public void Damage(int amount) {
        CurrentHealth = Math.Max(CurrentHealth - amount, 0);
        OnHealthChange(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0) {
            OnPlayerDied.Invoke();
        }
    }
    
    public void Heal(int amount) {
        CurrentHealth = Math.Min(CurrentHealth + amount, MaxHealth);
        OnHealthChange(CurrentHealth, MaxHealth);
    }
    
    public void IncreaseMaxHealth(int amount) {
        MaxHealth += amount;
        OnHealthChange(CurrentHealth, MaxHealth);
    }
}
