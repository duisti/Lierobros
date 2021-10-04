using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 8f;
	[Tooltip("How fast we accelerate to the desired speed. Big number means faster. Recommended at least 4-5 times of speed.")]
	public float acceleration = 32f;
	[Range(0.00001f, 1)]
	[Tooltip("Penalty applied to force inputs to left & right while in air. Values between 0.0001 - 1 only. Low(<0.05) = no control, high(>0.10) = lots of control control." +
		" Keep in mind that air has no drag, so the value has to be very low for an effect.")]
	public float airPenalty = 0.05f;
	public float jumpForce = 27.5f;
    private Rigidbody2D rg;
	public GameObject sprite;
	[HideInInspector]
	public bool isGrounded = true;
	//debug
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
		float _acceleration = acceleration;
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
			//apply air penalty, just a nerf to acceleration
			if (!isGrounded) {
				_acceleration *= airPenalty;
			}
			rg.velocity = new Vector2(Mathf.MoveTowards(rg.velocity.x, movement.x, _acceleration), rg.velocity.y);
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
