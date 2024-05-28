using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class ToasterArenaPlatform : MonoBehaviour
{
    [SerializeField]
    private float resetDelay;

    [SerializeField]
    private Sprite enabledSprite;
    [SerializeField]
    private Sprite disabledSprite;

    private Rigidbody2D rb2d;

    private void Enable(bool value) {
        GetComponent<BoxCollider2D>().enabled = value;
        GetComponent<SpriteRenderer>().sprite = value ? enabledSprite : disabledSprite;
    }

    IEnumerator TimedReset() {
        yield return new WaitForSeconds(resetDelay);
        Enable(true);
    }

    public void Trigger() {
        Enable(false);
        StartCoroutine(TimedReset());
    }

    void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        rb2d.gravityScale = 0;
        Enable(true);
    }
}
