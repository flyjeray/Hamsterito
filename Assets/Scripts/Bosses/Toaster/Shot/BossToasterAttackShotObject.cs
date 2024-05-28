using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossToasterAttackShotObject : BossAttackObject 
{
    private bool isLaunched = false;
    [SerializeField]
    private float speed = 5;

    private float lifetime = 5;

    IEnumerator TimedDestroy() {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void Awake() {
        GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.layer = 9;
    }

    public void Launch() {
        isLaunched = true;
        transform.SetParent(null);
        StartCoroutine(TimedDestroy());
    }

    void FixedUpdate() {
        if (isLaunched) {
            transform.position = Vector3.MoveTowards(
                transform.position,
                transform.position + (Vector3.up * 100),
                speed * Time.fixedDeltaTime
            );
        }
    }

    void OnCollisionEnter2D(Collision2D collider) {
        GameObject obj = collider.gameObject;

        ToasterArenaPlatform platform = obj.GetComponent<ToasterArenaPlatform>();
        PlayerHealthManager health = obj.GetComponent<PlayerHealthManager>();

        if (platform) {
            platform.Trigger();
        }
        if (health) {
            health.ModifyHealth(-1);
        }

        Destroy(gameObject);
    }
} 