using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossVacuumAttackSmashObject : MonoBehaviour
{
    [SerializeField]
    private float setupSpeed = 30;
    [SerializeField]
    private float attackSpeed = 65;

    private float speed;

    private Vector3 setupPos;
    private float landingY;
    private Vector3 target;

    void Awake() {
        GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.layer = 7;
    }

    IEnumerator TimedDestroyEnumerator(float t) {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }

    public void Setup(Vector3 pos, float floorY, float lifetime) {
        setupPos = pos;
        landingY = floorY;
        transform.position = new Vector3(setupPos.x, setupPos.y + 20, setupPos.z);
        StartCoroutine(TimedDestroyEnumerator(lifetime));
    }

    public void SetReady() {
        speed = setupSpeed;
        target = setupPos;
    }

    public void Launch() {
        speed = attackSpeed;
        target = new Vector3(
            transform.position.x,
            landingY,
            transform.position.z
        );
    }

    void FixedUpdate() {
        if (target != null) {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D (Collision2D collider) {
        if (collider.gameObject.tag == "Player") {
            HealthManager healthManager = collider.gameObject.GetComponent<HealthManager>();

            if (healthManager) {
                healthManager.ModifyHealth(-1);
            }

            Destroy(gameObject);
        }
    }
}
