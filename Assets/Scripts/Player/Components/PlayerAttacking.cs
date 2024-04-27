using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private bool isAiming;
    private PlayerCamera playerCamera;

    void Awake() {
        playerCamera = GetComponent<PlayerCamera>();
    }

    void Update() {
        isAiming = Input.GetMouseButton(1);
        Cursor.visible = isAiming;

        if (playerCamera != null) {
            playerCamera.UpdateAimingState(isAiming);
        }
    }
}
