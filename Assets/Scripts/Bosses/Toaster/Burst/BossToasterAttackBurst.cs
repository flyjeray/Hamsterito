using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Boss))]
[RequireComponent(typeof(BossFaceRotation))]
public class BossToasterAttackBurst : BossAttack
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject projectileSpawnMarker;

    [SerializeField]
    private int amountOfProjectiles = 3;
    [SerializeField]
    private float delayBetweenShots = .3f;

    [SerializeField]
    private float rotationDelay = .3f;

    private Boss boss;
    private BossFaceRotation rotator;

    void Awake() {
        boss = GetComponent<Boss>();
        rotator = GetComponent<BossFaceRotation>();
    }

    public override IEnumerator Action(int phase)
    {  
        Player player = FindObjectOfType<Player>();
        rotator.SetActive(true);
        boss.LockMovement(true);
        yield return new WaitForSeconds(rotationDelay);

        for (int i = 0; i < amountOfProjectiles; i++) {
            GameObject projectile = Instantiate(
                projectilePrefab,
                projectileSpawnMarker.transform.position,
                Quaternion.identity
            );
            BossToasterAttackBurstObject component = projectile.GetComponent<BossToasterAttackBurstObject>();
            if (component && player) {
                component.Launch(player.transform.position);
            }
            yield return new WaitForSeconds(delayBetweenShots);
        }

        yield return new WaitForSeconds(rotationDelay);
        rotator.SetActive(false);
        boss.LockMovement(false);
    }
}