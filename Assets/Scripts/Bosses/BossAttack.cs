using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    [SerializeField]
    protected float attackStartDelay = .5f;
    [SerializeField]
    protected float attackEndDelay = .5f;
    [SerializeField]
    protected int minPhase = 1;
    [SerializeField]
    protected int maxPhase = 1;

    public bool IsAvailableOnPhase(int phase) {
        return minPhase <= phase && maxPhase >= phase;
    }

    public abstract IEnumerator Action();

    public IEnumerator Execute() {
        yield return new WaitForSeconds(attackStartDelay);
        yield return StartCoroutine(Action());
        yield return new WaitForSeconds(attackEndDelay);
        GetComponent<Boss>().ExecuteRandomAttack();
    }
}
