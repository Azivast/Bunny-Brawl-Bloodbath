using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    [Tooltip("Curve of shake strength.")] [SerializeField]
    private AnimationCurve curve;
    
    private static bool shaking;
    private static float duration = 1f;
    private static float intensity = 1f;
    
    public static Vector3 ShakeOffset;
    
    private void Update() {
        if (shaking) {
            shaking = false;
            StartCoroutine(DoShake(duration, intensity));
        }
    }

    private IEnumerator DoShake(float duration, float intensity = 1) {
        var elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            var modifier = curve.Evaluate(elapsedTime / duration) * intensity;
            ShakeOffset = (Vector3)Random.insideUnitCircle * modifier;
            yield return null;
        }

        ShakeOffset = Vector3.zero;
    }

    public static void Shake(float duration, float intensity = 1) {
        shaking = true;
        CameraShake.duration = duration;
        CameraShake.intensity = intensity;
    }
}

