using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectDropper : MonoBehaviour {
    [SerializeField] private GameObject[] objects;
    [SerializeField] private float spread = 1;

    public void DropItems() {
        foreach (GameObject obj in objects) {
            Instantiate(obj, transform.position + (Vector3)Random.insideUnitCircle*spread, Quaternion.identity);
        }

        objects = Array.Empty<GameObject>();
    }
}
