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
    private float aimAngle = 0;
    private float aimLineWidth = .1f;
    private float aimSens = 500;

    private bool isShotReady = true;
    private float shotDelay = 0.5f;

    private int currAmmo = 1;
    private int maxAmmo = 6;
    private bool maxAmmoOnSpawn = true;
    
    private float reloadDelay = 0.5f;
    private bool isReloading = false;

    private PlayerCamera playerCamera;
    private PlayerMovement movement;
    private Coroutine reloadCoroutine;
    private LineRenderer aimLine;

    public GameObject weaponPrefab;
    private GameObject weapon;
    private SpriteRenderer weaponRenderer;

    void Awake() {
        playerCamera = GetComponent<PlayerCamera>();
        movement = GetComponent<PlayerMovement>();

        weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weapon.transform.SetParent(transform);
        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponRenderer.sortingOrder = 1;

        aimLine = GetComponent<LineRenderer>();
        aimLine.enabled = false;
        aimLine.startWidth = aimLineWidth;
        aimLine.endWidth = aimLineWidth;
        aimLine.material = new Material(Shader.Find("Sprites/Default"));
        aimLine.startColor = Color.red;
        aimLine.endColor = Color.red;

        if (maxAmmoOnSpawn) {
            currAmmo = maxAmmo;
        }

        Cursor.visible = false;
    }

    void Shoot(RaycastHit2D hit) {
        isShotReady = false;
        currAmmo--;
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
        while (currAmmo < maxAmmo) {
            isReloading = true;
            yield return new WaitForSeconds(reloadDelay);
            currAmmo++;
        }
        isReloading = false;
    }

    void Update() {
        isAiming = Input.GetMouseButton(1);

        if (playerCamera != null) {
            playerCamera.UpdateAimingState(isAiming);
        }
        
        weaponRenderer.enabled = isAiming;

        if (isAiming) {
            float resultAngle = movement.IsFacingRight() ? -aimAngle : aimAngle;
            
            Vector3 aimLineStart = weapon.transform.GetChild(0).position;
            Vector3 aimLineDir = Quaternion.Euler(0, 0, resultAngle) * Vector3.up;

            weaponRenderer.transform.localScale = new Vector3(1, movement.IsFacingRight() ? 1 : -1, 1);
            weapon.transform.rotation = Quaternion.Euler(0, 0, resultAngle + 90);

            RaycastHit2D hit = Physics2D.Raycast(aimLineStart, aimLineDir, aimingDistance, ~LayerMask.GetMask("Player"));
            aimLine.SetPositions(new Vector3[]{ aimLineStart, hit.collider ? hit.point : aimLineStart + aimLineDir * aimingDistance });

            if (Input.GetMouseButtonDown(0) && isShotReady && currAmmo > 0) {
                if (reloadCoroutine != null) {
                    isReloading = false;
                    StopCoroutine(reloadCoroutine);
                }
                Shoot(hit);
            }

            aimAngle = Math.Clamp(aimAngle - Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * aimSens, 0, 180);
        } else if (Input.GetKeyDown(KeyCode.R) && !isReloading) {
            reloadCoroutine = StartCoroutine(Reload());
        }

        aimLine.enabled = isAiming;
    }

    public bool IsAiming() { return isAiming; }
}
