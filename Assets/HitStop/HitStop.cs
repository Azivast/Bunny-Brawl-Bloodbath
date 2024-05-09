using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private static bool trigger;
    private static bool active;
    private static float duration;
    
    private IEnumerator DoStop(float duration)
    {
        Time.timeScale = 0.0f;
        active = true;
        yield return new WaitForSecondsRealtime(duration);
        duration = 0;
        Time.timeScale = 1.0f;
        active = false;
    }
    
    private void Update() {
        if (trigger) {
            trigger = false;
            StartCoroutine(DoStop(duration));
        }
    }
    
    public static void Stop(float stopDuration)
    {
        if (active) return;
        HitStop.duration = stopDuration;
        HitStop.trigger = true;
    }
}
