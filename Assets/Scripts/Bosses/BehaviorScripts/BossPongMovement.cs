using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossPongMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject positionX;
    [SerializeField]
    private GameObject positionY;

    private bool movingToX = false;
    
    [SerializeField]
    private float speed;

    [SerializeField]
    private float minAwaitTimeBeforeChangeDirection = 2;
    [SerializeField]
    private float maxAwaitTimeBeforeChangeDirection = 4;

    IEnumerator ChangeDirection() {
        while (true) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minAwaitTimeBeforeChangeDirection, maxAwaitTimeBeforeChangeDirection));
            movingToX = !movingToX;
        }
    }

    void Start() {
        StartCoroutine(ChangeDirection());
    }

    void FixedUpdate() {
        if (GetComponent<Boss>().IsEnabled()) {
            transform.position = Vector3.MoveTowards(
                transform.position,
                movingToX ? positionX.transform.position : positionY.transform.position,
                speed * Time.fixedDeltaTime
            );
        }
    }
}
