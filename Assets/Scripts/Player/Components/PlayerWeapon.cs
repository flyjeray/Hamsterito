using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerWeapon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerUI playerUI;
    private PlayerAudio playerAudio;
    [SerializeField]
    private float cameraShakeIntensity = 15;
    
    [SerializeField]
    private int currAmmo = 1;
    [SerializeField]
    private int maxAmmo = 6;

    private bool isShotReady = true;
    [SerializeField]
    private float shotDelay = 0.5f;
    
    [SerializeField]
    private float reloadDelay = 0.5f;
    [SerializeField]
    private bool isReloadingByOne = false;
    private bool isReloading = false;

    private Coroutine reloadCoroutine;

    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject projectileSpawnPoint;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 2;
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

    public void BindComponents(PlayerUI ui, PlayerAudio audio) {
        playerUI = ui;
        playerAudio = audio;
    }

    IEnumerator ShotDelayEnumerator()
    {
        yield return new WaitForSeconds(shotDelay);
        isShotReady = true;
    }
    
    public void Shoot(Vector3 target, Collider2D collider) {
        if (reloadCoroutine != null) {
            isReloading = false;
            StopCoroutine(reloadCoroutine);
        }
        
        GameObject gameObject = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity);
        Projectile projectile = gameObject.GetComponent<Projectile>();
        projectile.SetDirection(target, collider);
        isShotReady = false;
        currAmmo--;
        playerUI.UpdateBulletsVisibility(currAmmo);
        playerAudio.PlayShot();
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
            playerAudio.PlayReload();
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
}
