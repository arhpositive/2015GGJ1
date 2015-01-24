using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
	Vector3 initial_pos;
	void OnTriggerExit(Collider other) {
		if (other.name == "SceneBounds") {
			transform.position = initial_pos;
			rigidbody.velocity = Vector3.zero;
		}
	}

	void Start () {
		initial_pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
