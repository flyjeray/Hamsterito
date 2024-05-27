using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ToasterArenaPlatform : MonoBehaviour
{
    [SerializeField]
    private float resetDelay;

    [SerializeField]
    private Sprite enabledSprite;
    [SerializeField]
    private Sprite disabledSprite;

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
}
