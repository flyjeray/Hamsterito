using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BossVacuumAttackRainDropHealth))]
public class BossVacuumAttackRainDrop : MonoBehaviour
{
    private float spinDelta;
    
    [SerializeField]
    private float fallSpeed;

    void Awake() {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        spinDelta = Random.Range(1, 5) * Random.Range(0, 2) == 0 ? 1 : -1;
    }

    void FixedUpdate() {
        transform.position = new Vector2(transform.position.x, transform.position.y - fallSpeed * Time.fixedDeltaTime);
        transform.Rotate(0, 0, spinDelta);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            HealthManager health = collision.gameObject.GetComponent<HealthManager>();
            if (health) {
                health.ModifyHealth(-1);
            }
        }
        Destroy(gameObject);
    }
}
