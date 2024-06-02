using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector3 movementDirection;

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;

    IEnumerator TimedDestroyEnumerator(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        gameObject.layer = 8;
        StartCoroutine(TimedDestroyEnumerator(3));
    }

    public void SetDirection(Vector3 target, Collider2D sender) {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), sender);
        Vector3 dir = target - transform.position;
        dir = new Vector3(dir.x, dir.y, 0);

        if (dir.magnitude > 1) {
            dir = dir.normalized;
        } else if (dir.magnitude > 0) {
            dir = dir / dir.magnitude;
        }

        movementDirection = dir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FixedUpdate() {
        rb2d.MovePosition(transform.position + movementDirection * speed);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();

        if (healthManager) {
            healthManager.ModifyHealth(-damage);
        }

        Destroy(gameObject);
    }
}
