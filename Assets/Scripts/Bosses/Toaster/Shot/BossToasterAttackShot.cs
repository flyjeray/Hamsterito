using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossToasterAttackShot : BossAttack
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject spawnMarker;
    [SerializeField]
    private float baseDelayBeforeLaunch = 1.25f;

    public override IEnumerator Action(int phase)
    {
        PhaseParameters parameters = FindPhase(phase);

        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnMarker.transform.position,
            Quaternion.identity
        );
        projectile.transform.SetParent(transform);

        BossToasterAttackShotObject component = projectile.GetComponent<BossToasterAttackShotObject>();

        if (component) {
            yield return new WaitForSeconds(baseDelayBeforeLaunch / parameters.objectSpeedMultiplier);
            if (component) {
                component.Launch();
            }
        } else {
            yield return new WaitForSeconds(0);
        }
    }
}
