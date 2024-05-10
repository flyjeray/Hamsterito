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
    
    public override IEnumerator Action()
    {
        bool rightSide = Random.Range(0, 2) == 0;
        GameObject gameObject = Instantiate(
            objectPrefab, 
            rightSide ? rightSpawnMarker.transform.position : leftSpawnMarker.transform.position,
            Quaternion.identity
        );
        
        BossVacuumAttackPullingObject component = gameObject.GetComponent<BossVacuumAttackPullingObject>();

        if (component) {
            component.Setup(rightSide, rightSpawnMarker.transform.position, leftSpawnMarker.transform.position);
            component.SetReady();
            yield return new WaitForSeconds(3f);
            component.Launch();
            yield return new WaitForSeconds(5f);
            if (gameObject) {
                Destroy(gameObject);
            }
        } else {
            yield return new WaitForSeconds(0);
        }
    }
}
