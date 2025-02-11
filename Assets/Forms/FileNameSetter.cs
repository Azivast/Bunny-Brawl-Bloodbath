using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileNameSetter : MonoBehaviour
{
    private void Awake()
    {
        SaveData.Filename = Time.time.ToString();
    }
}
