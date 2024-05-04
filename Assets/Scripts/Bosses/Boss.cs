using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class Boss : MonoBehaviour
{
    private List<BossAttack> attacks;
    private int lastAttackIndex;

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

    void Awake() {
        attacks = gameObject.GetComponents<BossAttack>().ToList();
        ExecuteRandomAttack();
    }
}
