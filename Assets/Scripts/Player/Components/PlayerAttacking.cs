using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerAttacking : MonoBehaviour
{
    private bool isAiming;
    private float aimingDistance = 5;
    private float aimingAngle = 0;
    private float aimLineWidth = .1f;
    private float aimingSens = 500;

    private bool isShotReady = true;
    private float shotDelay = 0.5f;

    private int currentAmmo = 1;
    private int maxAmmo = 6;
    private bool maxAmmoOnSpawn = true;
    
    private float reloadDelay = 0.5f;
    private bool isReloading = false;

    private PlayerCamera playerCamera;
    private PlayerMovement playerMovement;
    private Coroutine reloadCoroutine;
    private LineRenderer lineRenderer;

    void Awake() {
        playerCamera = GetComponent<PlayerCamera>();
        playerMovement = GetComponent<PlayerMovement>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.startWidth = aimLineWidth;
        lineRenderer.endWidth = aimLineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        if (maxAmmoOnSpawn) {
            currentAmmo = maxAmmo;
        }

        Cursor.visible = false;
    }

    void Shoot(RaycastHit2D hit) {
        isShotReady = false;
        currentAmmo--;
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

        if (playerCamera != null) {
            playerCamera.UpdateAimingState(isAiming);
        }

        if (isAiming) {
            Vector3 aimingDirection = Quaternion.Euler(0, 0, playerMovement.IsFacingRight() ? -aimingAngle : aimingAngle) * Vector3.up;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimingDirection, aimingDistance, ~LayerMask.GetMask("Player"));
            lineRenderer.SetPositions(new Vector3[]{ transform.position, hit.collider ? hit.point : transform.position + aimingDirection * aimingDistance });
            if (Input.GetMouseButtonDown(0) && isShotReady && currentAmmo > 0) {
                if (reloadCoroutine != null) {
                    isReloading = false;
                    StopCoroutine(reloadCoroutine);
                }
                Shoot(hit);
            }
            aimingAngle = Math.Clamp(aimingAngle - Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * aimingSens, 0, 180);
        } else if (Input.GetKeyDown(KeyCode.R) && !isReloading) {
            reloadCoroutine = StartCoroutine(Reload());
        }

        lineRenderer.enabled = isAiming;
    }

    public bool IsAiming() { return isAiming; }
}
