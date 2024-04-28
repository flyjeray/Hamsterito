using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private float movement;

    public float Speed = 5;
    public float JumpForce = 300;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = new PhysicsMaterial2D{ friction = 0, name = "PLAYER - Physics Material" };

        box = GetComponent<BoxCollider2D>();
    }

    void Update() {
        movement = Input.GetAxisRaw("Horizontal");
        Vector2 direction = new Vector3(transform.position.x, box.bounds.min.y, transform.position.z) - transform.position;
        float distance = Vector2.Distance(transform.position, box.bounds.min);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Ground"));
        if (Input.GetKeyDown(KeyCode.Space) && hit.collider) {
            rb.AddForce(Vector2.up * JumpForce);
        }
    }

    void FixedUpdate() {
        rb.velocity = new Vector2(movement * Speed, rb.velocity.y);
    }
}
