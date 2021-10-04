using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 8f;
	[Range(0.00001f, 1)]
	[Tooltip("Penalty applied to force inputs to left & right while in air. Values between 0.0001 - 1 only. Low = no control, high = all control")]
	public float airPenalty = 0.2f;
	public float jumpForce = 27.5f;
    private Rigidbody2D rg;
	public GameObject sprite;
	//debug
	[HideInInspector]
	public bool isGrounded = true;
	[HideInInspector]
	public float moveDir = 0;
	
	public enum Direction {
		left = -1,
		right = 1,
		none = 0
	}

	Direction InputDirection = Direction.none;
    
    void Awake() {
		rg = GetComponent<Rigidbody2D>();
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
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * speed, 0f);
		if (movement.x < 0) {
			InputDirection = Direction.left;
		}
		if (movement.x > 0) {
			InputDirection = Direction.right;
		}
		if (Mathf.Approximately(movement.x, 0)) {
			InputDirection = Direction.none;
		}
		moveDir = movement.x;
		//old implementation, it works but it's disabled because it didn't allow input in to opposite direction while in air and there's velocity towards the other direction
		//if ((InputDirection == Direction.left && Mathf.Abs(rg.velocity.x) > Mathf.Abs(movement.x)) ||
		//	(InputDirection == Direction.right && Mathf.Abs(rg.velocity.x) > Mathf.Abs(movement.x))) {
		if ((InputDirection == Direction.left && rg.velocity.x > movement.x) ||
			(InputDirection == Direction.right && rg.velocity.x < movement.x)) {
			if (!isGrounded) {
				movement *= airPenalty;
			}
			rg.velocity = new Vector2(rg.velocity.x, rg.velocity.y) + movement;
		}
		SpriteFlipper();
    }

    void SpriteFlipper() {
        if (InputDirection != Direction.none) {
            sprite.transform.localScale = new Vector3(Mathf.Clamp((float)InputDirection, -1f, 1f), sprite.transform.localScale.y, sprite.transform.localScale.z);
        } 
    }

    void Jump() {
		//old implementation, it's disabled because the jump height varied too much when climbing slopes
		//rg.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
		rg.velocity = new Vector2(rg.velocity.x, jumpForce);
    }

    void OnTriggerStay2D(Collider2D col) {
        isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D col) {
        isGrounded = false;
    }
}
