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
    private int currentPhase = 1;
    private bool active = true;
    private int[] lastAttacksBuffer = new int[] {};
    private int attackBufferLength;

    public void SelectCurrentAttacks() {
        currentAttacks.Clear();
        for (int i = 0; i < attacks.Count; i++) {
            if (attacks[i].IsAvailableOnPhase(currentPhase)) {
                currentAttacks.Add(attacks[i]);
            }
        }
        attackBufferLength = Math.Clamp(currentAttacks.Count - 1, 1, 3);
        lastAttacksBuffer = new int[attackBufferLength];
    }

    public void ExecuteRandomAttack() {
        if (currentAttacks.Count == 0 || !active) {
            return;
        }

        if (currentAttacks.Count > 1) {
            System.Random rnd = new System.Random();
            int i = 0;
            while (lastAttacksBuffer.Contains(i)) {
                i = rnd.Next(0, currentAttacks.Count);
            }
            
            if (lastAttacksBuffer.Length >= attackBufferLength) {
                int[] newArr = new int[lastAttacksBuffer.Length];
                Array.Copy(lastAttacksBuffer, 1, newArr, 0, lastAttacksBuffer.Length - 1);
                newArr[lastAttacksBuffer.Length - 1] = i;
                lastAttacksBuffer = newArr;
            } else {
                lastAttacksBuffer[lastAttacksBuffer.Length] = i;
            }

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

    public void Enable(bool enabled) {
        active = enabled;
    }

    public bool IsEnabled() { return active; }
}
