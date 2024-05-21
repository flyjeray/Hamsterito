using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Canvas canvas;
    private GameObject canvas_go;
    private float distanceBetweenBulletAndPlayer = 1;

    [SerializeField]
    private Sprite bullet;
    private List<GameObject> bullets;

    [SerializeField]
    private GameObject pressToStartCanvasPrefab;
    private Canvas pressToStartCanvas;
    private bool levelStarted = false;

    void Awake() {
        canvas_go = new GameObject("PLAYER - Canvas");
        canvas = canvas_go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 3;
        canvas_go.AddComponent<CanvasScaler>();
        canvas_go.transform.SetParent(transform);

        pressToStartCanvas = Instantiate(pressToStartCanvasPrefab).GetComponent<Canvas>();
        Time.timeScale = 0;
    }

    public void SetActive(bool isActive) {
        canvas.enabled = isActive;
    }

    public void SetupBullets(int amount, int activeAmount) {
        bullets = new List<GameObject>{};
        for (int i = 1; i <= amount; i++) {
            GameObject bullet_go = new GameObject("PLAYER - Canvas - Bullet " + i);
            bullets.Add(bullet_go);

            Image img = bullet_go.AddComponent<Image>();
            img.sprite = bullet;

            bullet_go.transform.localScale = new Vector3(1, 1, 1);
            bullet_go.GetComponent<RectTransform>().sizeDelta = new Vector2(.4f, .4f);
            bullet_go.transform.Rotate(new Vector3(0, 0, 0), 360 / amount * i);
            bullet_go.transform.SetParent(canvas_go.transform);

            Vector3 dir = Vector3.up * distanceBetweenBulletAndPlayer;
            Quaternion angle = Quaternion.Euler(0, 0, 360 / amount * i);
            dir = angle * dir;
            bullet_go.transform.position = dir;
            bullet_go.transform.rotation = angle;

            if (i > activeAmount) {
                bullet_go.SetActive(false);
            }
        }
    }

    public void UpdateBulletsVisibility(int activeAmount) {
        for (int i = 0; i < bullets.Count; i++) {
            bullets[i].SetActive(activeAmount > i);
        }
    }

    void Update() {
        if (Input.anyKey && !levelStarted) {
            pressToStartCanvas.enabled = false;
            Time.timeScale = 1;
            levelStarted = true;
            FindAnyObjectByType<Boss>().Enable(true);
        }
    }
}
