using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVacuumAttackSmash : BossAttack
{
    [SerializeField]
    private GameObject objectPrefab;

    [SerializeField]
    private GameObject topHeightMarker;
    [SerializeField]
    private GameObject floorHeightMarker;
    [SerializeField]
    private float delayBeforeLaunch = .7f;
    
    public override IEnumerator Action(int phase)
    {
        PhaseParameters currPhase = FindPhase(phase);
        Player player = FindAnyObjectByType<Player>();
        Vector3 pos = new Vector3(
            player.gameObject.transform.position.x,
            topHeightMarker.transform.position.y,
            0
        );
        GameObject gameObject = Instantiate(
            objectPrefab,
            new Vector3(
                pos.x,
                pos.y + 10000,
                pos.z
            ),
            Quaternion.identity
        );
        
        BossVacuumAttackSmashObject component = gameObject.GetComponent<BossVacuumAttackSmashObject>();

        if (component) {
            component.Setup(pos, floorHeightMarker.transform.position.y, delayBeforeLaunch + .5f, currPhase.objectSpeedMultiplier);
            component.SetReady();
            yield return new WaitForSeconds(delayBeforeLaunch);
            component.Launch();
        } else {
            yield return new WaitForSeconds(0);
        }
    }
}
