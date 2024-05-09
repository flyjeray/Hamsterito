using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossVacuumAttackSweepObjectBase : MonoBehaviour
{
    protected void Awake() {
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
    
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            HealthManager health = collision.gameObject.GetComponent<HealthManager>();

            if (health) {
                health.ModifyHealth(-1);
            }
            
            Destroy(transform.parent.gameObject);
        }
    }
}
