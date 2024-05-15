using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathZone : MonoBehaviour
{
    void Awake() {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            PlayerHealthManager healthManager = collider.gameObject.GetComponent<PlayerHealthManager>();

            if (healthManager) {
                healthManager.ModifyHealth(-100);
            }
        }
    }
}
