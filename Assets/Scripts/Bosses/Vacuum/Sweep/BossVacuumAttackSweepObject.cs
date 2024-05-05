using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BossVacuumAttackSweepObject : MonoBehaviour
{
    [SerializeField]
    private GameObject leftSpawnMarker;
    [SerializeField]
    private GameObject rightSpawnMarker;

    void Awake() {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void Setup(bool spawnOnRightSide) {
        GetComponent<SpriteRenderer>().flipX = spawnOnRightSide;
        Vector3 basePosition = spawnOnRightSide ? rightSpawnMarker.transform.position : leftSpawnMarker.transform.position;
        transform.position = new Vector3(
            basePosition.x + (spawnOnRightSide ? 5000 : -5000),
            basePosition.y,
            basePosition.z
        );
    }

    public void Start() {}
}
