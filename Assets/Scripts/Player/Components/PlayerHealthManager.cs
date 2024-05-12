using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerHealthManager : HealthManager
{
    protected override void OnLethalDamageTaken()
    {
        GetComponent<Animator>().SetTrigger("Death");
        GetComponent<PlayerMovement>().EnableMovement(false);
        Boss boss = FindAnyObjectByType<Boss>();
        if (boss) {
            boss.EnableAttacks(false);
        }
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
