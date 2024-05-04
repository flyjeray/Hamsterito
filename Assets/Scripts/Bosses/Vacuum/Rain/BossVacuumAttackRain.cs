using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVacuumAttackRain : BossAttack
{
    [SerializeField]
    private List<GameObject> spawnMarkers;

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

    private int lastUsedSpawnMarker;

    private void CreateDrop() {
        int drop = Random.Range(0, drops.Count);
        int marker = Random.Range(0, spawnMarkers.Count);
        while (marker == lastUsedSpawnMarker && spawnMarkers.Count > 1) {
            marker = Random.Range(0, spawnMarkers.Count);
        }
        lastUsedSpawnMarker = marker;

        Instantiate(drops[drop], spawnMarkers[marker].transform.position, Quaternion.identity);
    }

    public override IEnumerator Action()
    {
        int amount = Random.Range(minAmountOfDrops, maxAmountOfDrops);
        for (int i = 0; i < amount; i++) {
            yield return new WaitForSeconds(Random.Range(minDelayBetweenDrops, maxDelayBetweenDrops));
            CreateDrop();
        }
    }
}
