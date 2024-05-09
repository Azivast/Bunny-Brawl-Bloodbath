using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public static class ConstRandom
{
    public static Random Random;

    public static void Initialize(int seed)
    {
        Random = new Random(seed);
    }
}
