using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 30f;
    private Rigidbody2D rigidBody;
    public bool isGrounded = true;
	[HideInInspector]
    public float moveDir = 0;
    public GameObject sprite;
    
    void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
		if (sprite == null) {
			sprite = GameObject.Find("Sprite");
		}
    }

    void FixedUpdate() {
        Movement();

        if ((Input.GetKey(KeyCode.Space) || Input.GetButton("Jump")) && isGrounded) {
            Jump();
        }
    }

    void Movement() {
		Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), 0f);
        playerPos += movement * Time.deltaTime * speed;
		transform.position = playerPos;
        moveDir = movement.x;
        SpriteFlipper(moveDir);
    }

    void SpriteFlipper(float moveDir) {
        if (moveDir > 0) {
            sprite.transform.localScale = new Vector3(1, sprite.transform.localScale.y, sprite.transform.localScale.z);
        } 
        if (moveDir < 0) {
            sprite.transform.localScale = new Vector3(-1, sprite.transform.localScale.y, sprite.transform.localScale.z);
        }
    }

    void Jump() {
        rigidBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    void OnTriggerStay2D(Collider2D col) {
        isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D col) {
        isGrounded = false;
    }
}
