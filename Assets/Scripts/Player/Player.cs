using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerVisual))]
[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(PlayerAttacking))]
[RequireComponent(typeof(PlayerUI))]
[RequireComponent(typeof(PlayerHealthManager))]
[RequireComponent(typeof(PlayerAudio))]
public class Player : MonoBehaviour {
    public bool active;

    public bool IsActive() { return active; }

    public void SetActive(bool value) { active = value; }
    
    void Awake() {
        gameObject.tag = "Player";
        gameObject.name = "PLAYER";
        gameObject.layer = 6;
        active = false;
    }
}
