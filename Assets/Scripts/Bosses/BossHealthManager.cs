using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Boss))]
[RequireComponent(typeof(Animator))]
public class BossHealthManager : HealthManager
{
    [Serializable]
    protected class Phase {
        public int order;
        public int minHealthInclusive;
        public int maxHealthInclusive;
        public string animationName;
    }

    [SerializeField]
    private string deathAnimationName;

    [SerializeField]
    protected List<Phase> phases;

    [SerializeField]
    private float relatedLevel;

    private IEnumerator SpriteBlinkEnumerator() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer) {
            Color color = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = color;
        }
        yield return new WaitForSeconds(0);
    }

    private void UpdatePhase() {
        for (int i = 0; i < phases.Count; i++) {
            if (
                currentHealth <= phases[i].maxHealthInclusive &&
                currentHealth >= phases[i].minHealthInclusive
            ) {
                GetComponent<Boss>().SetPhase(phases[i].order);
                GetComponent<Animator>().Play(phases[i].animationName);
            }
        }
    }

    protected override void OnNonLethalDamageTaken(int damage) {
        StartCoroutine(SpriteBlinkEnumerator());
        UpdatePhase();
    }

    void Start() {
        UpdatePhase();
    }

    void DestroyAllObjects() {
        BossAttackObject[] objects = FindObjectsByType<BossAttackObject>(FindObjectsSortMode.None);

        for (int i = 0; i < objects.Length; i++) {
            Destroy(objects[i].gameObject);
        }
    }

    protected override void OnLethalDamageTaken() {
        DestroyAllObjects();
        FindObjectOfType<Player>().SetActive(false);
        GetComponent<Boss>().Enable(false);
        GetComponent<Animator>().Play(deathAnimationName);
    }

    public void Hide() {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void BackToMenu() {
        PlayerPrefs.SetInt("LVL" + relatedLevel, 1);
        SceneManager.LoadScene("LevelSelectScreen");
    }
}
