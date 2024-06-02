using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerAttacking))]
[RequireComponent(typeof(PlayerVisual))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAttacking playerAttacking;
    private Player player;
    private PlayerVisual playerVisual;
    private BoxCollider2D box;
    private Animator animator;
    private float movement;
    private bool facingRight = true;

    public float Speed = 6.5f;
    public float AimingSpeedMultiplier = 0.85f;
    public float JumpForce = 400;

    private bool movable = true;

    void Awake() {
        playerAttacking = GetComponent<PlayerAttacking>();
        playerVisual = GetComponent<PlayerVisual>();
        player = GetComponent<Player>();

        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = new PhysicsMaterial2D{ friction = 0, name = "PLAYER - Physics Material" };

        box = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (player.IsActive()) {
            movement = Input.GetAxisRaw("Horizontal");
            animator.SetBool("Moving", movement != 0);
            if (!playerAttacking.IsAiming() && movement != 0 && movable) {
                playerVisual.FaceSpriteRight(movement < 0);
                facingRight = movement > 0;
            } else if (playerAttacking.IsAiming() && movable) {
                Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                playerVisual.FaceSpriteRight(cursorPos.x < transform.position.x);
                facingRight = cursorPos.x >= transform.position.x;
            }
            Vector2 direction = new Vector3(transform.position.x, box.bounds.min.y, transform.position.z) - transform.position;
            float distance = Vector2.Distance(transform.position, box.bounds.min);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Ground", "IgnorableGround"));
            if (Input.GetKeyDown(KeyCode.Space) && hit.collider && movable) {
                rb.AddForce(Vector2.up * JumpForce);
            }
            Physics2D.IgnoreLayerCollision(6, 10, Input.GetKey(KeyCode.S));
        }
    }

    public bool IsFacingRight() { return facingRight; }

    void FixedUpdate() {
        if (player.IsActive()) {
            rb.isKinematic = false;
            rb.velocity = new Vector2(movement * Speed * (playerAttacking.IsAiming() ? AimingSpeedMultiplier : 1), rb.velocity.y);
        } else {
            rb.isKinematic = true;
            rb.velocity = new Vector2(0, 0);
        }
    }

    public Collider2D GetCollider2D() { return GetComponent<BoxCollider2D>(); }

    public void EnableMovement(bool enabled) {
        movable = enabled;
    }
}
