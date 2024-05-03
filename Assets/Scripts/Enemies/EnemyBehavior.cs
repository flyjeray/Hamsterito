using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    protected EnemyFieldOfView fov;
    protected GameObject playerGameObject;
    protected bool isPlayerInFOV;
    protected bool isPlayerNoticed;

    void Awake() {
        fov.BindBehavior(this);
        gameObject.layer = 7;
    }
    
    public virtual void OnPlayerEnterFOV(GameObject player) {
        isPlayerInFOV = true;
        playerGameObject = player;
    }

    public virtual void OnPlayerExitFOV() {
        isPlayerInFOV = false;
        playerGameObject = null;
        if (isPlayerNoticed) {
            isPlayerNoticed = false;
            OnPlayerLost();
        }
    }

    protected abstract void OnPlayerNoticed();
    protected abstract void OnPlayerLost();

    private void TrackPlayer() {
        if (isPlayerInFOV) {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                playerGameObject.transform.position - transform.position,
                Mathf.Infinity,
                ~LayerMask.GetMask("Enemy", "Ignore Raycast")
            );

            if (hit.collider && hit.collider.tag == "Player") {
                if (!isPlayerNoticed) {
                    isPlayerNoticed = true;
                    OnPlayerNoticed();
                }
            } else {
                if (isPlayerNoticed) {
                    isPlayerNoticed = false;
                    OnPlayerLost();
                }
            }
        }
    }

    void Update() {
        TrackPlayer();
    }
}
