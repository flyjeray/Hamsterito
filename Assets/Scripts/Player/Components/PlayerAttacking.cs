using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private bool isAiming;

    private bool isShotReady = true;
    private float shotDelay = 0.5f;

    private int currentAmmo = 1;
    private int maxAmmo = 6;
    private bool maxAmmoOnSpawn = true;
    
    private float reloadDelay = 0.5f;
    private bool isReloading = false;

    private PlayerCamera playerCamera;
    private Coroutine reloadCoroutine;

    void Awake() {
        playerCamera = GetComponent<PlayerCamera>();

        if (maxAmmoOnSpawn) {
            currentAmmo = maxAmmo;
        }
    }

    void Shoot() {
        isShotReady = false;
        currentAmmo--;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position, 500, ~LayerMask.GetMask("Player"));
        if (hit.collider) {
            HealthManager health = hit.collider.GetComponent<HealthManager>();

            if (health) {
                health.ModifyHealth(-1);
            }
        }
        StartCoroutine(ShotDelay());
    }

    IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(shotDelay);
        isShotReady = true;
    }

    IEnumerator Reload() {
        while (currentAmmo < maxAmmo) {
            isReloading = true;
            yield return new WaitForSeconds(reloadDelay);
            currentAmmo++;
        }
        isReloading = false;
    }

    void Update() {
        isAiming = Input.GetMouseButton(1);
        Cursor.visible = isAiming;

        if (playerCamera != null) {
            playerCamera.UpdateAimingState(isAiming);
        }

        if (isAiming && Input.GetMouseButtonDown(0) && isShotReady && currentAmmo > 0) {
            if (reloadCoroutine != null) {
                isReloading = false;
                StopCoroutine(reloadCoroutine);
            }
            Shoot();
        } else if (Input.GetKeyDown(KeyCode.R) && !isReloading) {
            reloadCoroutine = StartCoroutine(Reload());
        }
    }
}
