using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    [SerializeField]
    protected float attackStartDelay = .5f;
    [SerializeField]
    protected float attackEndDelay = .5f;

    public abstract IEnumerator Action();

    public IEnumerator Execute() {
        yield return new WaitForSeconds(attackStartDelay);
        yield return StartCoroutine(Action());
        yield return new WaitForSeconds(attackEndDelay);
        GetComponent<Boss>().ExecuteRandomAttack();
    }
}
