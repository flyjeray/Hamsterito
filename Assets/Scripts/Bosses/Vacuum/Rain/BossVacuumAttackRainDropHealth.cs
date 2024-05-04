using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BossVacuumAttackRainDropHealth : HealthManager
{
    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator SpriteRedBlink() {
        Color defaultColor = spriteRenderer.color;
        yield return new WaitForSeconds(.2f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.2f);
        spriteRenderer.color = defaultColor;
    }

    protected override void OnNonLethalDamageTaken(int damage) {
        StartCoroutine(SpriteRedBlink());
    }
}
