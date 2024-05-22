using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenClickListener : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    void Update()
    {
        if (Input.anyKey) {
            SceneManager.LoadScene(sceneName);
        }
    }
}
