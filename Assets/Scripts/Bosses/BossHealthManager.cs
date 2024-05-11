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
        public int minHealthInclusive;
        public int maxHealthInclusive;
        public Sprite bossVisual;
    }

    [SerializeField]
    protected List<Phase> phases;

    private IEnumerator SpriteBlinkEnumerator() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer) {
            Color color = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = color;
        }
        yield return new WaitForSeconds(0);
    }

    protected override void OnNonLethalDamageTaken(int damage) {
        StartCoroutine(SpriteBlinkEnumerator());
        for (int i = 0; i < phases.Count; i++) {
            if (
                currentHealth <= phases[i].maxHealthInclusive &&
                currentHealth >= phases[i].minHealthInclusive
            ) {
                GetComponent<Boss>().SetPhase(phases[i].order);
                GetComponent<Boss>().SelectCurrentAttacks();
                GetComponent<SpriteRenderer>().sprite = phases[i].bossVisual;
            }
        }
    }
}
