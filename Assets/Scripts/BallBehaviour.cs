using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
	Vector3 initial_pos;
	GameObject camera;

	void OnTriggerExit(Collider other) {
		if (other.name == "SceneBounds") {
			transform.position = initial_pos;
			rigidbody.velocity = Vector3.zero;
		}
	}

	void Start () {
		initial_pos = transform.position;
		camera = GameObject.Find ("Main Camera");
	}
	
	// Update is called once per frame 
	void Update () {
		float distance = Vector3.Distance (camera.transform.position, transform.position);

		camera.transform.position = transform.position - rigidbody.velocity.normalized * 3.0f + Vector3.up*1.0f ;
		camera.transform.rotation = Quaternion.LookRotation (rigidbody.velocity, Vector3.up);
	
	}
}
