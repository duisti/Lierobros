using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
	[SerializeField]
	private Transform target;
	public float cameraSpeed = 15f; //base camera speed
	public float maxDistance = 15f; //camera will never fall behind this distance
	float divider = 1.5f; //this is used for the camera catchup

    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	private void LateUpdate() {
		MoveCamera();
	}

	private void MoveCamera() {
		if (target == null) {
			return;
		}
		Vector2 finalPosition = target.position;
		Vector2 currentPosition = transform.position;
		//check if we are way too far. If too far, we will move much faster towards our goal
		/*
		if (Vector2.Distance(finalPosition, currentPosition) > maxDistance) {
			currentPosition = Vector2.MoveTowards(currentPosition, finalPosition, Vector2.Distance(finalPosition, currentPosition) - maxDistance);
		}
		*/
		Vector2 lerpPosition = Vector2.Lerp(currentPosition, finalPosition, cameraSpeed * Time.deltaTime);
		transform.position = lerpPosition;
	}

	public void SetTarget(Transform t) {
		target = t;
	}
}
