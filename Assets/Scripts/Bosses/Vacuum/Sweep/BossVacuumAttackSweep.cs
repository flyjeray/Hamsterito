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
    
    public override IEnumerator Action()
    {
        bool rightSide = Random.Range(0, 2) == 0;
        GameObject gameObject = Instantiate(
            objectPrefab, 
            rightSide ? rightSpawnMarker.transform.position : leftSpawnMarker.transform.position,
            Quaternion.identity
        );
        
        BossVacuumAttackSweepObject component = gameObject.GetComponent<BossVacuumAttackSweepObject>();

        if (component) {
            component.Setup(rightSide, rightSpawnMarker.transform.position, leftSpawnMarker.transform.position);
            yield return new WaitForSeconds(0);
            component.SetReady();
            yield return new WaitForSeconds(3f);
            component.Launch();
        } else {
            yield return new WaitForSeconds(0);
        }
    }
}
