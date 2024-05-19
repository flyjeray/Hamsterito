using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackObjectHealthManager : HealthManager
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    new void Awake() {
        base.Awake();
        if (!spriteRenderer) {
            SpriteRenderer neighborRenderer = GetComponent<SpriteRenderer>();

            if (neighborRenderer) {
                spriteRenderer = neighborRenderer;
            }
        }
    }

    private IEnumerator SpriteBlinkEnumerator() {
        if (spriteRenderer) {
            Color color = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = color;
        }
        yield return new WaitForSeconds(0);
    }

    protected override void OnNonLethalDamageTaken(int damage)
    {
        StartCoroutine(SpriteBlinkEnumerator());
    }
}
