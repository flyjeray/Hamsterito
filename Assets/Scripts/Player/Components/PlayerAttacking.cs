using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerUI))]
public class PlayerAttacking : MonoBehaviour
{
    [SerializeField]
    private bool isAiming;
    private bool maxAmmoOnSpawn = true;

    private PlayerCamera playerCamera;
    private PlayerMovement movement;
    private PlayerUI playerUI;

    public GameObject weaponPrefab;
    private PlayerWeapon weapon;

    void Awake() {
        playerCamera = GetComponent<PlayerCamera>();
        movement = GetComponent<PlayerMovement>();
        playerUI = GetComponent<PlayerUI>();

        GameObject weapon_go = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weapon_go.transform.SetParent(transform);
        weapon = weapon_go.GetComponent<PlayerWeapon>();
        weapon.BindUI(playerUI);

        if (maxAmmoOnSpawn && weapon) {
            weapon.ReloadInstantly();
        }
    }

    void Start() {
        playerUI.SetActive(isAiming);
    }

    IEnumerator ShakeCamera(float intensity, float duration) {
        playerCamera.SetShake(intensity);
        yield return new WaitForSeconds(duration);
        playerCamera.SetShake(0);
    }

    void Update() {

        if (playerCamera != null) {
            playerCamera.UpdateAimingState(isAiming);
        }

        if (Input.GetKeyDown(KeyCode.R) && weapon && !weapon.IsReloading() && weapon.IsReloadNeeded()) {
            weapon.Reload();
        } else if (isAiming && weapon) {
            Vector3 cursorPointOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            weapon.FaceSpriteRight(movement.IsFacingRight());
            weapon.transform.LookAt(new Vector3(
                cursorPointOnScreen.x,
                cursorPointOnScreen.y,
                weapon.transform.position.z
            ));

            if (Input.GetMouseButton(0) && weapon.IsAbleToShoot()) {
                weapon.Shoot(cursorPointOnScreen, movement.GetCollider2D());
                StartCoroutine(ShakeCamera(weapon.GetCameraShakeIntensity(), 0.2f));
            }
        }
        
        if (weapon) {
            weapon.SwitchVisibility(isAiming);
        }
    }

    public bool IsAiming() { return isAiming; }
}
