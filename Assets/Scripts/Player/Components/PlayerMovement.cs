using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float movement;

    public float Speed = 3;
    public float JumpForce = 500;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        movement = Input.GetAxisRaw("Horizontal");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
        if (Input.GetKeyDown(KeyCode.Space) && hit.collider) {
            rb.AddForce(Vector2.up * JumpForce);
        }
    }

    void FixedUpdate() {
        rb.velocity = new Vector2(movement * Speed, rb.velocity.y);
    }
}
