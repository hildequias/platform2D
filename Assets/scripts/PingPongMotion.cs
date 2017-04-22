using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMotion : MonoBehaviour {

	// This transformation
	private Transform thisTransform = null;

	// Original position
	private Vector3 origPos = Vector3.zero;

	// Axes to Move on
	public Vector3 moveAxes = Vector2.zero;

	// Speed
	public float distance = 3f;

	void Awake() {
		// Get Transform component
		thisTransform = GetComponent<Transform> ();

		// Copy original position
		origPos = thisTransform.position;
	}
	
	// Update is called once per frame
	void Update () {

        // Update platform position with ping pong
		thisTransform.position = origPos + moveAxes * Mathf.PingPong (Time.time, distance);
	}
}
