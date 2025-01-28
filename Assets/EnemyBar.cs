using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBar : MonoBehaviour
{
    [SerializeField] ObjectCollection enemiesAlive;
    [SerializeField] TMP_Text text;
    private int maxEnemies;

    private void Start()
    {
        maxEnemies = enemiesAlive.GetObjects().Count;
    }

    private void Update()
    {
        //text.text = "Enemies: " + enemiesAlive.GetObjects().Count.ToString() + "/" + maxEnemies;
        text.text = (maxEnemies-enemiesAlive.GetObjects().Count).ToString() + "/" + maxEnemies;
    }
}
