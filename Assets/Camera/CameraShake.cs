using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    [Tooltip("Curve of shake strength.")] [SerializeField]
    private AnimationCurve curve;
    
    private static bool shaking;
    private static float duration = 1f;
    private static float intensity = 1f;
    private static Vector2 direction;
    
    public static Vector3 ShakeOffset;
    
    private void Update() {
        if (shaking) {
            shaking = false;
            StartCoroutine(DoShake(duration, direction, intensity));
        }
    }

    private IEnumerator DoShake(float duration, Vector2 direction, float intensity = 1) {
        var elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            var modifier = curve.Evaluate(elapsedTime / duration) * intensity;
            ShakeOffset = direction * modifier;
            yield return null;
        }

        ShakeOffset = Vector3.zero;
    }

    public static void Shake(float duration, float intensity = 1) {
        shaking = true;
        CameraShake.duration = duration;
        CameraShake.intensity = intensity;
        CameraShake.direction = (Vector3)Random.insideUnitCircle ;
    }

    public static void Shake(float duration, Vector2 direction, float intensity = 1, float randomness = 0.4f)  {
        shaking = true;
        CameraShake.duration = duration;
        CameraShake.intensity = intensity;
        CameraShake.direction = direction*0.3f + Random.insideUnitCircle*randomness;
    }
}

