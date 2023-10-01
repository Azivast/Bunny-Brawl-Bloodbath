using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UprightRotationHandler : MonoBehaviour
{
    void Update()
    {
        transform.rotation = quaternion.identity;
    }
}
