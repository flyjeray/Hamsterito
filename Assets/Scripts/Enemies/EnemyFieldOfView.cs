using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    private EnemyBehavior owner;

    void Awake() {
        gameObject.layer = 2;
    }

    public void BindBehavior(EnemyBehavior behavior) {
        owner = behavior;
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.gameObject.tag == "Player" && owner) {
            owner.OnPlayerEnterFOV(collider2D.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.gameObject.tag == "Player") {
            owner.OnPlayerExitFOV();
        }
    }
}
