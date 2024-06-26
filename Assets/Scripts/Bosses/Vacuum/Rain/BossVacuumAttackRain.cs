using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVacuumAttackRain : BossAttack
{
    [SerializeField]
    private GameObject leftSpawnMarker;
    [SerializeField]
    private GameObject rightSpawnMarker;

    [SerializeField]
    private List<GameObject> drops;

    [SerializeField]
    private int minAmountOfDrops = 10;
    [SerializeField]
    private int maxAmountOfDrops = 15;
    [SerializeField]
    private float minDelayBetweenDrops = .5f;
    [SerializeField]
    private float maxDelayBetweenDrops = 1f;

    private void CreateDrop(float speedMultiplier) {
        int drop = Random.Range(0, drops.Count);
        float x = Random.Range(leftSpawnMarker.transform.position.x, rightSpawnMarker.transform.position.x);

        GameObject newDrop = Instantiate(
            drops[drop], 
            new Vector3(x, leftSpawnMarker.transform.position.y, 0), 
            Quaternion.identity
        );

        newDrop.GetComponent<BossVacuumAttackRainDrop>().SetSpeedMultiplier(speedMultiplier);
    }

    public override IEnumerator Action(int phase)
    {
        PhaseParameters currPhase = FindPhase(phase);
        int amount = Random.Range(minAmountOfDrops, maxAmountOfDrops);
        for (int i = 0; i < amount; i++) {
            yield return new WaitForSeconds(Random.Range(minDelayBetweenDrops, maxDelayBetweenDrops));
            CreateDrop(currPhase.objectSpeedMultiplier);
        }
    }
}
