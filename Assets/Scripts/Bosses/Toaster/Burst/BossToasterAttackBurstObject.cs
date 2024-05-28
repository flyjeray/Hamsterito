using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossToasterAttackBurstObject : BossAttackObject
{
    private bool isLaunched = false;
    [SerializeField]
    private float speed = 20;

    private float lifetime = 5;
    private Vector3 target;

    IEnumerator TimedDestroy() {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void Awake() {
        GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.layer = 9;
    }

    public void Launch(Vector3 _target) {
        isLaunched = true;
        transform.SetParent(null);
        target = transform.position + (_target - transform.position) * 1000;
        StartCoroutine(TimedDestroy());
    }

    void FixedUpdate() {
        if (isLaunched) {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
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