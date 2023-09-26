using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {
    [SerializeField] private Image bar;
    [SerializeField] private TMP_Text text;
    [SerializeField] private PlayerHealthObject healthObject;

    private void OnEnable() {
        healthObject.OnHealthChange += UpdateBar;
        UpdateBar(healthObject.CurrentHealth, healthObject.MaxHealth);
    }

    private void OnDisable() {
        healthObject.OnHealthChange -= UpdateBar;
    }

    private void UpdateBar(int newHealth, int maxHealth) {
        bar.fillAmount = (float)newHealth/maxHealth;

        text.text = $"{newHealth}/{maxHealth}";
    }
}
