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

    void Awake() {
        player = FindObjectOfType<Player>();
    }

    void FixedUpdate() {
        if (GetComponent<Boss>().IsEnabled() && player) {
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
