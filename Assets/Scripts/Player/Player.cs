using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerVisual))]
[RequireComponent(typeof(PlayerCamera))]
public class Player : MonoBehaviour {
    void Awake() {
        gameObject.tag = "Player";
        gameObject.name = "PLAYER";
    }
}
