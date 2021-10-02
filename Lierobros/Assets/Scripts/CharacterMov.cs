using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMov : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 400f;
    private Rigidbody2D rigidBody;
    public bool isGrounded = true;
    public float spriteRotation = 1;
    public float moveDir = 0;
    
    void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Movement() {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * speed;
        moveDir = movement.x;
    }

    void Jump() {
        rigidBody.AddForce(transform.up * jumpForce);
    }

    void OnTriggerStay2D(Collider2D col) {
        isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D col) {
        isGrounded = false;
    }
}
