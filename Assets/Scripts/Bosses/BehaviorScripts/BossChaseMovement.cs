using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossChaseMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private bool chaseHorizontal;
    [SerializeField]
    private bool chaseVertical;

    private Player player;
    private Boss boss;

    void Awake() {
        player = FindObjectOfType<Player>();
        boss = GetComponent<Boss>();
    }

    void FixedUpdate() {
        if (boss.IsEnabled() && !boss.IsMovementLocked() && player) {
            Vector3 destination = new Vector3(
                chaseHorizontal ? player.transform.position.x : transform.position.x,
                chaseVertical ? player.transform.position.y : transform.position.y,
                transform.position.z
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                speed * Time.fixedDeltaTime
            );
        }
    }
}
