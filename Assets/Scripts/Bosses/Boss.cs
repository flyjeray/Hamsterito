using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BossHealthManager))]
public class Boss : MonoBehaviour
{
    private List<BossAttack> attacks;
    private List<BossAttack> currentAttacks = new List<BossAttack>{};
    private int lastAttackIndex;
    private int currentPhase = 1;

    protected void SelectCurrentAttacks() {
        currentAttacks.Clear();
        for (int i = 0; i < attacks.Count; i++) {
            if (attacks[i].IsAvailableOnPhase(currentPhase)) {
                currentAttacks.Add(attacks[i]);
            }
        }
    }

    public void ExecuteRandomAttack() {
        if (attacks.Count == 0) {
            return;
        }

        if (attacks.Count > 1) {
            System.Random rnd = new System.Random();
            int i = 0;
            while (i == lastAttackIndex) {
                i = rnd.Next(0, attacks.Count);
            }
            lastAttackIndex = i;
            StartCoroutine(attacks[i].Execute());
        } else {
            StartCoroutine(attacks[0].Execute());
        }
    }

    public void SetPhase(int phase) {
        currentPhase = phase;
    }

    void Awake() {
        attacks = gameObject.GetComponents<BossAttack>().ToList();
        SelectCurrentAttacks();
        ExecuteRandomAttack();
    }
}
