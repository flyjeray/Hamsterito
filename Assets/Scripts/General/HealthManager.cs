using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    protected int currentHealth = 1;

    [SerializeField]
    protected int maxHealth = 5;

    [SerializeField]
    protected bool maxOnSpawn = true;

    protected void Awake() {
        if (maxOnSpawn) {
            currentHealth = maxHealth;
        }
    }

    protected virtual void OnLethalDamageTaken() {
        Destroy(gameObject);
    }

    protected virtual void OnNonLethalDamageTaken(int damage) {}

    protected virtual void OnHealed(int heal) {}

    public void ModifyHealth(int delta) {
        currentHealth = Math.Clamp(currentHealth + delta, 0, maxHealth);

        if (currentHealth == 0) {
            OnLethalDamageTaken();
        } else if (delta < 0) {
            OnNonLethalDamageTaken(delta);
        } else if (delta > 0) {
            OnHealed(delta);
        }
    }
}
