using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVacuumAttackPulling : BossAttack
{
    [SerializeField]
    private GameObject objectPrefab;

    [SerializeField]
    private GameObject leftSpawnMarker;
    [SerializeField]
    private GameObject rightSpawnMarker;

    private float maxLifespan = 5;
    private float checkFrequency = 0.25f;

    [SerializeField]
    private float delayBeforeLaunch = 2.25f;

    public override IEnumerator Action(int phase)
    {
        PhaseParameters currPhase = FindPhase(phase);

        bool rightSide = Random.Range(0, 2) == 0;
        GameObject pullGameObject = Instantiate(
            objectPrefab, 
            rightSide ? rightSpawnMarker.transform.position : leftSpawnMarker.transform.position,
            Quaternion.identity
        );
        
        BossVacuumAttackPullingObject component = pullGameObject.GetComponent<BossVacuumAttackPullingObject>();
        
        float lifetime = 0;

        if (component) {
            component.Setup(rightSide, rightSpawnMarker.transform.position, leftSpawnMarker.transform.position, currPhase.objectSpeedMultiplier);
            component.SetReady();
            yield return new WaitForSeconds(delayBeforeLaunch);
            component.Launch();
            while (pullGameObject) {
                yield return new WaitForSeconds(checkFrequency);
                lifetime += checkFrequency;
                if (lifetime > maxLifespan) {
                    Destroy(pullGameObject);
                }
            }
        } else {
            yield return new WaitForSeconds(0);
        }
    }
}
