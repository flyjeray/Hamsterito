using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class MenuLevelButton : MonoBehaviour
{
    [SerializeField]
    private int level;
    [SerializeField]
    private int requiredLevel;
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private bool developed = false;

    private bool open = false;

    private void Awake() {
        if (!developed) {
            GetComponent<SpriteRenderer>().color = Color.black;
            open = false;
        } else if (PlayerPrefs.GetInt("LVL" + level) == 1) {
            GetComponent<SpriteRenderer>().color = Color.green;
            open = true;
        } else if (PlayerPrefs.GetInt("LVL" + requiredLevel) == 1 || level == 1) {
            GetComponent<SpriteRenderer>().color = Color.white;
            open = true;
        } else {
            GetComponent<SpriteRenderer>().color = Color.red;
            open = false;
        }
    }

    private void OnClick() {
        if (open) {
            Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(ray, ray);
            if (hit.collider.gameObject == gameObject)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private void FixedUpdate() {
        if (Input.GetMouseButtonDown(0)) {
            OnClick();
        }
    }
}
