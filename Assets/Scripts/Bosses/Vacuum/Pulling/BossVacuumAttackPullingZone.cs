using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BossVacuumAttackPullingZone : MonoBehaviour
{
    [SerializeField]
    private float pullForce;
    private bool pullEnabled;
    
    private GameObject player;

    void Awake() {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void Enable() {
        pullEnabled = true;
    }

    void OnTriggerEnter2D (Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            player = collider.gameObject;
        }
    }

    void OnTriggerExit2D (Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            player = null;
        }
    }

    void FixedUpdate() {
        if (player && pullEnabled) {
            bool right = player.transform.position.x > transform.position.x;
            player.transform.position = new Vector3(
                player.transform.position.x + (right ? -1 : 1) * pullForce * Time.fixedDeltaTime,
                player.transform.position.y,
                player.transform.position.z
            );
        }
    }
}
