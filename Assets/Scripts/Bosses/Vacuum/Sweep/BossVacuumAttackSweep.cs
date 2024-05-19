using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVacuumAttackSweep : BossAttack
{
    [SerializeField]
    private GameObject objectPrefab;

    [SerializeField]
    private GameObject leftSpawnMarker;
    [SerializeField]
    private GameObject rightSpawnMarker;

    [SerializeField]
    private float delayBeforeLaunch = 3f;

    private float maxLifespan = 5;
    private float checkFrequency = 0.25f;
    
    public override IEnumerator Action(int phase)
    {
        PhaseParameters currPhase = FindPhase(phase);

        bool rightSide = Random.Range(0, 2) == 0;
        GameObject sweepGameObject = Instantiate(
            objectPrefab, 
            rightSide ? rightSpawnMarker.transform.position : leftSpawnMarker.transform.position,
            Quaternion.identity
        );
        
        BossVacuumAttackSweepObject component = sweepGameObject.GetComponent<BossVacuumAttackSweepObject>();

        float lifetime = 0;

        if (component) {
            component.Setup(
                rightSide, 
                rightSpawnMarker.transform.position, 
                leftSpawnMarker.transform.position,
                currPhase.objectSpeedMultiplier
            );
            component.SetReady();
            yield return new WaitForSeconds(delayBeforeLaunch);
            if (component) {
                component.Launch();
            }
            while (sweepGameObject) {
                yield return new WaitForSeconds(checkFrequency);
                lifetime += checkFrequency;
                if (lifetime > maxLifespan) {
                    Destroy(sweepGameObject);
                }
            }
        } else {
            yield return new WaitForSeconds(0);
        }
    }
}
