using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
	[SerializeField]
	private Transform target;
	public float cameraSpeed = 15f; //base camera speed

    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	private void FixedUpdate() {
		MoveCamera();
	}

	private void MoveCamera() {
		if (target == null) {
			return;
		}
		Vector2 finalPosition = target.position;
		Vector2 currentPosition = transform.position;
		Vector2 lerpPosition = Vector2.Lerp(currentPosition, finalPosition, cameraSpeed * Time.deltaTime);
		transform.position = lerpPosition;
	}

	public void SetTarget(Transform t) {
		target = t;
	}
}
