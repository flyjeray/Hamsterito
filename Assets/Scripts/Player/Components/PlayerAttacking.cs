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

    private bool maxAmmoOnSpawn = true;

    private PlayerCamera playerCamera;
    private PlayerMovement movement;
    private LineRenderer aimLine;

    public GameObject weaponPrefab;
    private PlayerWeapon weapon;

    void Awake() {
        playerCamera = GetComponent<PlayerCamera>();
        movement = GetComponent<PlayerMovement>();

        GameObject weapon_go = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weapon_go.transform.SetParent(transform);
        weapon = weapon_go.GetComponent<PlayerWeapon>();

        aimLine = GetComponent<LineRenderer>();
        aimLine.enabled = false;
        aimLine.startWidth = aimLineWidth;
        aimLine.endWidth = aimLineWidth;
        aimLine.material = new Material(Shader.Find("Sprites/Default"));
        aimLine.startColor = Color.red;
        aimLine.endColor = Color.red;
        aimLine.sortingOrder = 1;

        if (maxAmmoOnSpawn && weapon) {
            weapon.ReloadInstantly();
        }

        Cursor.visible = false;
    }

    IEnumerator ShakeCamera(float intensity, float duration) {
        playerCamera.SetShake(intensity);
        yield return new WaitForSeconds(duration);
        playerCamera.SetShake(0);
    }

    void Update() {
        isAiming = Input.GetMouseButton(1);

        if (playerCamera != null) {
            playerCamera.UpdateAimingState(isAiming);
        }

        if (Input.GetKeyDown(KeyCode.R) && weapon && !weapon.IsReloading()) {
            weapon.Reload();
        } else if (isAiming && weapon) {
            float resultAngle = movement.IsFacingRight() ? -aimAngle : aimAngle;
            
            Vector3 aimLineStart = transform.position;
            if (weapon.transform.childCount > 0) {
                aimLineStart = weapon.transform.GetChild(0).position;
            }

            Vector3 aimLineDir = Quaternion.Euler(0, 0, resultAngle) * Vector3.up;

            weapon.FaceSpriteRight(movement.IsFacingRight());
            weapon.transform.rotation = Quaternion.Euler(0, 0, resultAngle + 90);

            RaycastHit2D hit = Physics2D.Raycast(aimLineStart, aimLineDir, aimingDistance, ~LayerMask.GetMask("Player"));
            aimLine.SetPositions(new Vector3[]{ aimLineStart, hit.collider ? hit.point : aimLineStart + aimLineDir * aimingDistance });

            if (Input.GetMouseButton(0) && weapon.IsAbleToShoot()) {
                weapon.Shoot(hit);
                StartCoroutine(ShakeCamera(weapon.GetCameraShakeIntensity(), 0.2f));
            }

            aimAngle = Math.Clamp(aimAngle - Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * aimSens, 0, 180);
        }
        
        if (weapon) {
            weapon.SwitchVisibility(isAiming);
            aimLine.enabled = isAiming;
        }
    }

    public bool IsAiming() { return isAiming; }
}
