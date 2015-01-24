﻿using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
	Vector3 initial_pos;
	GameObject camera;

	void OnTriggerExit(Collider other) {
		if (other.name == "SceneBounds") {
			transform.position = initial_pos;
			rigidbody.velocity = Vector3.up*0.01f;
			reset ();
		}
	}

	void Start () {
		initial_pos = transform.position;
		camera = GameObject.Find ("Main Camera");
	}

	public void reset(){
		camera.transform.rotation = Quaternion.LookRotation (rigidbody.velocity, Vector3.up);

	}
	// Update is called once per frame 
	void Update () {
		Quaternion target_quat = Quaternion.LookRotation (rigidbody.velocity, Vector3.up);

		float angle_diff = Quaternion.Angle(camera.transform.rotation, target_quat);
		float max_angle_rot = Time.deltaTime*360;

		if(angle_diff < max_angle_rot){
			camera.transform.rotation = target_quat;
		}else{
			camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, target_quat, max_angle_rot/angle_diff);
		}

		camera.transform.position = transform.position + camera.transform.rotation*Vector3.forward * -3.5f + Vector3.up * 1.5f;
	}
}
