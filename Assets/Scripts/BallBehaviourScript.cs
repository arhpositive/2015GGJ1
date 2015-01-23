using UnityEngine;
using System.Collections;

public class BallBehaviourScript : MonoBehaviour {

	
	public string left_move_key = "left";
	public string right_move_key = "right";
	public bool reversed = false;
	public const float MAX_MOVE = 12.0f;




	float time_since_move_start;
	bool moving_left = false;
	bool moving_right = false;
	float move_speed = 0.0f;
	void Start () {
	
	}


	void Update () {
		if (Input.GetKeyUp (left_move_key)) {
			moving_left = false;
		}
		if (Input.GetKeyDown (left_move_key)) {
			moving_left = true;
			time_since_move_start = 0.0f;
		}
		
		if (Input.GetKeyUp (right_move_key)) {
			moving_right = false;
		}
		if (Input.GetKeyDown (right_move_key)) {
			moving_right = true;
			time_since_move_start = 0.0f;
		}

		if (moving_left && moving_right) {
			time_since_move_start = 0.0f; 
		} else if (moving_left || moving_right) {
			time_since_move_start += Time.deltaTime;
			move_speed = Mathf.Clamp01 (time_since_move_start / 0.5f) * 30.0f;
			if (moving_left) {
					move_speed = -move_speed;
			}
			
			transform.Translate (move_speed * Time.deltaTime, 0, 0);
		} else {
			move_speed *= Mathf.Pow(0.90f, Time.deltaTime/0.016f);
			if(Mathf.Abs(move_speed) < 0.001f){
				move_speed = 0.0f;
			}
			transform.Translate (move_speed * Time.deltaTime, 0, 0);
		}

		float x = transform.position.x;
		if (Mathf.Abs (x) > MAX_MOVE) {
			x = Mathf.Clamp(x, -MAX_MOVE, MAX_MOVE);
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
		}
		float a = (reversed?-1.0f:1.0f)*Mathf.Clamp (x / 10.0f, -1.0f, 1.0f);
		a = Mathf.Sign (a) * Mathf.Pow (Mathf.Abs (a), 2.0f);
		float left_angle, right_angle;
		if (a < 0) {
			left_angle = a * 50.0f;
			right_angle = a * 30.0f;
		} else {
			left_angle = a * 30.0f;
			right_angle = a * 50.0f;
		}
		
		transform.FindChild ("left_arm").rotation = Quaternion.AngleAxis(left_angle, Vector3.up) * transform.rotation;
		transform.FindChild ("right_arm").rotation = Quaternion.AngleAxis(right_angle, Vector3.up) * transform.rotation;
	}
}
