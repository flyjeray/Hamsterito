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
    private bool active = false;
    private int[] buffer = new int[] {};
    private int maxBufferL;

    [SerializeField]
    private AudioClip backgroundMusic;
    private AudioSource backgroundMusicSource;

    public void SelectCurrentAttacks() {
        currentAttacks.Clear();
        for (int i = 0; i < attacks.Count; i++) {
            if (attacks[i].IsAvailableOnPhase(currentPhase)) {
                currentAttacks.Add(attacks[i]);
            }
        }
        maxBufferL = Math.Clamp(currentAttacks.Count - 1, 1, 3);
        buffer = new int[maxBufferL];
    }

    public void ExecuteRandomAttack() {
        if (currentAttacks.Count == 0 || !active) {
            return;
        }

        if (currentAttacks.Count > 1) {
            System.Random rnd = new System.Random();
            int i = 0;
            while (buffer.Contains(i)) {
                i = rnd.Next(0, currentAttacks.Count);
            }

            if (maxBufferL == 1) {
                buffer = new int[] {i};
            } else if (buffer.Length >= maxBufferL) {
                for (int y = 0; y < buffer.Length - 1; y++) {
                    buffer[y] = buffer[y + 1];
                }
                buffer[buffer.Length - 1] = i;
            } else {
                buffer[buffer.Length] = i;
            }

            StartCoroutine(currentAttacks[i].Execute(currentPhase));
        } else {
            StartCoroutine(currentAttacks[0].Execute(currentPhase));
        }
    }

    public void SetPhase(int phase) {
        if (phase != currentPhase) {
            currentPhase = phase;
            SelectCurrentAttacks();
        }
    }

    private void SetupMusic() {
        if (backgroundMusic) {
            GameObject gameObject = new GameObject("MUSIC SOURCE - " + name);
            backgroundMusicSource = gameObject.AddComponent<AudioSource>();
            gameObject.transform.SetParent(transform);
            backgroundMusicSource.loop = true;
            backgroundMusicSource.clip = backgroundMusic;
        }
    }

    void Awake() {
        attacks = gameObject.GetComponents<BossAttack>().ToList();
        gameObject.layer = 7;
        SelectCurrentAttacks();
        GetComponent<Rigidbody2D>().isKinematic = true;
        SetupMusic();
    }

    public void Enable(bool enabled) {
        active = enabled;

        if (enabled) {
            ExecuteRandomAttack();
            if (backgroundMusicSource) {
                backgroundMusicSource.Play();
            } 
        } else {
            if (backgroundMusicSource) {
                backgroundMusicSource.Stop();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        PlayerHealthManager health = collision.gameObject.GetComponent<PlayerHealthManager>();

        if (health) {
            health.ModifyHealth(-1);
        }
    }

    public bool IsEnabled() { return active; }
}
