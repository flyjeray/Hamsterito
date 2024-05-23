using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAudioSourceStarter : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    void Awake() {
        audioSource.Play();
    }
}
