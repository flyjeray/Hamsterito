using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    [Serializable]
    protected class PhaseParameters {
        public float minPhaseInclusive;
        public float maxPhaseInclusive;
        public float attackStartDelay;
        public float attackEndDelay;
        public float objectSpeedMultiplier;
    }

    [SerializeField]
    protected List<PhaseParameters> phases;

    protected PhaseParameters FindPhase(int phase) {
        return phases.Find(p => p.minPhaseInclusive >= phase && p.maxPhaseInclusive <= phase);
    }

    public bool IsAvailableOnPhase(int phase) {
        PhaseParameters found = FindPhase(phase);
        return found != null;
    }

    public abstract IEnumerator Action(int phase);

    public IEnumerator Execute(int phase) {
        PhaseParameters curr = FindPhase(phase);
        yield return new WaitForSeconds(curr.attackStartDelay);
        yield return StartCoroutine(Action(phase));
        yield return new WaitForSeconds(curr.attackEndDelay);
        GetComponent<Boss>().ExecuteRandomAttack();
    }
}
