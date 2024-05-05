using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossHealthManager : HealthManager
{
    [Serializable]
    protected class Phase {
        public int order;
        public int healthThreshold;
    }

    [SerializeField]
    protected List<Phase> phases;

    protected override void OnNonLethalDamageTaken(int damage) {
        int order = 1;
        for (int i = 0; i < phases.Count; i++) {
            if (currentHealth <= phases[i].healthThreshold) {
                order = phases[i].order;
            }
        }
        GetComponent<Boss>().SetPhase(order);
    }
}
