using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BossHealthManager))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Boss : MonoBehaviour
{
    private List<BossAttack> attacks;
    private List<BossAttack> currentAttacks = new List<BossAttack>{};
    private int lastAttackIndex;
    private int currentPhase = 1;

    public void SelectCurrentAttacks() {
        currentAttacks.Clear();
        for (int i = 0; i < attacks.Count; i++) {
            if (attacks[i].IsAvailableOnPhase(currentPhase)) {
                currentAttacks.Add(attacks[i]);
            }
        }
    }

    public void ExecuteRandomAttack() {
        if (currentAttacks.Count == 0) {
            return;
        }

        if (currentAttacks.Count > 1) {
            System.Random rnd = new System.Random();
            int i = 0;
            while (i == lastAttackIndex) {
                i = rnd.Next(0, currentAttacks.Count);
            }
            lastAttackIndex = i;
            StartCoroutine(currentAttacks[i].Execute(currentPhase));
        } else {
            StartCoroutine(currentAttacks[0].Execute(currentPhase));
        }
    }

    public void SetPhase(int phase) {
        currentPhase = phase;
    }

    void Awake() {
        attacks = gameObject.GetComponents<BossAttack>().ToList();
        gameObject.layer = 7;
        SelectCurrentAttacks();
        ExecuteRandomAttack();
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
