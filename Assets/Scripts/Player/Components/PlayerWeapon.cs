using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerWeapon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerUI playerUI;
    [SerializeField]
    private float cameraShakeIntensity = 15;
    [SerializeField]
    private float maxDistance = 5;
    
    [SerializeField]
    private int currAmmo = 1;
    [SerializeField]
    private int maxAmmo = 6;

    private bool isShotReady = true;
    [SerializeField]
    private float shotDelay = 0.5f;
    [SerializeField]
    private bool showHitMarker = true;
    
    [SerializeField]
    private float reloadDelay = 0.5f;
    [SerializeField]
    private bool isReloadingByOne = false;
    private bool isReloading = false;

    private Coroutine reloadCoroutine;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
    }

    void Start() {
        playerUI.SetupBullets(maxAmmo, currAmmo);
    }

    public void ReloadInstantly() {
        currAmmo = maxAmmo;
    }

    public void SwitchVisibility(bool show) {
        spriteRenderer.enabled = show;
    }

    public void FaceSpriteRight(bool facingRight) {
        // need to use scale instead of flipY not to mess with child barrel gameobject
        spriteRenderer.transform.localScale = new Vector3(1, facingRight ? 1 : -1, 1);
    }

    public bool IsAbleToShoot() {
        return isShotReady && currAmmo > 0;
    }

    public float GetCameraShakeIntensity() {
        return cameraShakeIntensity;
    }

    public void BindUI(PlayerUI ui) {
        playerUI = ui;
    }

    IEnumerator ShotDelayEnumerator()
    {
        yield return new WaitForSeconds(shotDelay);
        isShotReady = true;
    }
    
    public void Shoot(RaycastHit2D hit) {
        if (reloadCoroutine != null) {
            isReloading = false;
            StopCoroutine(reloadCoroutine);
        }

        isShotReady = false;
        currAmmo--;
        playerUI.UpdateBulletsVisibility(currAmmo);
        if (hit.collider) {
            HealthManager health = hit.collider.GetComponent<HealthManager>();

            if (health) {
                health.ModifyHealth(-1);
            }
        }
        StartCoroutine(ShotDelayEnumerator());
    }

    IEnumerator ReloadEnumerator() {
        while (currAmmo < maxAmmo) {
            isReloading = true;
            yield return new WaitForSeconds(reloadDelay);
            if (isReloadingByOne) {
                currAmmo++;
            } else {
                currAmmo = maxAmmo;
            }
            playerUI.UpdateBulletsVisibility(currAmmo);
        }
        isReloading = false;
    }

    public bool IsReloading() {
        return isReloading;
    }

    public bool IsReloadNeeded() {
        return currAmmo < maxAmmo;
    }

    public void Reload() {
        reloadCoroutine = StartCoroutine(ReloadEnumerator());
    }

    public float GetMaxDistance() {
        return maxDistance;
    }

    public bool IsShowingHitMarker() {
        return showHitMarker;
    }
}
