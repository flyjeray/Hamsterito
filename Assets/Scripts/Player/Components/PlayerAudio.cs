using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip shotSound;
    [SerializeField]
    private AudioClip reloadSound;

    private AudioSource shotSource;
    private AudioSource reloadSource;

    void Awake() {
        GameObject shotSourceGameObject = new GameObject("Player - Shot Sound Source");
        shotSource = shotSourceGameObject.AddComponent<AudioSource>();
        shotSource.loop = false;
        shotSource.clip = shotSound;
        shotSource.volume = .03f;

        GameObject reloadSourceGameObject = new GameObject("Player - Reload Sound Source");
        reloadSource = reloadSourceGameObject.AddComponent<AudioSource>();
        reloadSource.loop = false;
        reloadSource.clip = reloadSound;
        reloadSource.volume = .03f;
    }

    public void PlayShot() {
        shotSource.Play();
    }

    public void PlayReload() {
        reloadSource.Play();
    }
}
