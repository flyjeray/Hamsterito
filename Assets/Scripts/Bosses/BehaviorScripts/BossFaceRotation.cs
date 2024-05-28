using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossFaceRotation : MonoBehaviour
{
    private Quaternion initialRotation;
    private GameObject player;
    private bool active;

    [SerializeField]
    private float speed;

    void Awake() {
        initialRotation = transform.rotation;
        player = FindObjectOfType<Player>().gameObject;
    }

    void FixedUpdate() {
        Vector3 dir = active ? player.transform.position - transform.position : Vector2.up;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * speed);
    }

    public void SetActive(bool value) { 
        active = value;
    }
}