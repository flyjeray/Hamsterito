using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Player))]
public class PlayerHealthManager : HealthManager
{
    protected override void OnLethalDamageTaken()
    {
        GetComponent<Animator>().SetTrigger("Death");
        GetComponent<Player>().SetActive(false);
        GetComponent<PlayerCamera>().ZoomOnDeath();
        Boss boss = FindAnyObjectByType<Boss>();
        if (boss) {
            boss.Enable(false);
        }
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
