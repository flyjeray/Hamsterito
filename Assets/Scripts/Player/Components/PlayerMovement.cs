using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerAttacking))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAttacking playerAttacking;
    private BoxCollider2D box;
    private float movement;
    private bool facingRight = true;

    public float Speed = 5;
    public float AimingSpeedMultiplier = 0.5f;
    public float JumpForce = 300;

    void Awake() {
        playerAttacking = GetComponent<PlayerAttacking>();

        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = new PhysicsMaterial2D{ friction = 0, name = "PLAYER - Physics Material" };

        box = GetComponent<BoxCollider2D>();
    }

    void Update() {
        movement = Input.GetAxisRaw("Horizontal");
        if (movement > 0 && !playerAttacking.IsAiming()) {
            facingRight = true;
        } else if (movement < 0 && !playerAttacking.IsAiming()) {
            facingRight = false;
        }
        Vector2 direction = new Vector3(transform.position.x, box.bounds.min.y, transform.position.z) - transform.position;
        float distance = Vector2.Distance(transform.position, box.bounds.min);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Ground"));
        if (Input.GetKeyDown(KeyCode.Space) && hit.collider) {
            rb.AddForce(Vector2.up * JumpForce);
        }
    }

    public bool IsFacingRight() { return facingRight; }

    void FixedUpdate() {
        rb.velocity = new Vector2(movement * Speed * (playerAttacking.IsAiming() ? AimingSpeedMultiplier : 1), rb.velocity.y);
    }
}
