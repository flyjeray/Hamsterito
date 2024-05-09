using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossVacuumAttackSweepObjectHandle : HealthManager
{
    protected new void Awake() {
        base.Awake();
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    protected override void OnLethalDamageTaken()
    {
        Destroy(transform.parent.gameObject);
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
