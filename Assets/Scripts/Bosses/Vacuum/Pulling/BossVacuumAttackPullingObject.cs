using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossVacuumAttackPullingObject : HealthManager
{
    [SerializeField]
    private float setupSpeed = 30;

    private bool rightSide = false;
    
    private Vector3 target;
    private Vector3 right;
    private Vector3 left;

    protected new void Awake() {
        base.Awake();
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void Setup(bool spawnOnRightSide, Vector3 rightSpawn, Vector2 leftSpawn) {
        rightSide = spawnOnRightSide;
        right = rightSpawn;
        left = leftSpawn;
        transform.localScale = new Vector3(
            transform.localScale.x * (rightSide ? -1 : 1),
            transform.localScale.y,
            transform.localScale.z
        );
        Vector3 basePosition = rightSide ? right : left;
        transform.position = new Vector3(
            basePosition.x + (rightSide ? 50 : -50),
            basePosition.y,
            basePosition.z
        );
    }

    public void SetReady() {
        target = rightSide ? right : left;
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();

            if (healthManager) {
                healthManager.ModifyHealth(-1);
            }

            Destroy(gameObject);
        }
    }

    public void Launch() {
        GetComponentInChildren<BossVacuumAttackPullingZone>().Enable();
    }

    void FixedUpdate() {
        if (target != null) {
            transform.position = Vector3.MoveTowards(transform.position, target, setupSpeed * Time.fixedDeltaTime);
        }
    }
}
