using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVacuumAttackSweepObject : MonoBehaviour
{
    [SerializeField]
    private float setupSpeed = 30;
    [SerializeField]
    private float attackSpeed = 10;

    private float speed;

    private bool rightSide = false;
    
    private Vector3 target;
    private Vector3 right;
    private Vector3 left;

    void Awake() {
        gameObject.layer = 7;
    }

    public void Setup(bool spawnOnRightSide, Vector3 rightSpawn, Vector2 leftSpawn) {
        rightSide = spawnOnRightSide;
        right = rightSpawn;
        left = leftSpawn;
        transform.localScale = new Vector3(
            transform.localScale.x * (rightSide ? 1 : -1),
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
        speed = setupSpeed;
        target = rightSide ? right : left;
    }

    public void Launch() {
        speed = attackSpeed;
        target = rightSide ? left : right;
    }

    void FixedUpdate() {
        if (target != null) {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        }
    }
}
