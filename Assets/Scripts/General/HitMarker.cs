using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    public float lifetime = 0.1f;
    IEnumerator SelfDestroy() {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void Awake() {
        StartCoroutine(SelfDestroy());
    }
}
