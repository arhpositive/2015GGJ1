using UnityEngine;
using System.Collections;

public class BallBehaviourScript : MonoBehaviour {

	
	public string left_move_key = "left";
	public string right_move_key = "right";


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
			move_speed = Mathf.Clamp01 (time_since_move_start / 0.2f) * 10.0f;
			if (moving_left) {
					move_speed = -move_speed;
			}

			transform.Translate (0, move_speed * Time.deltaTime, 0);
		} else {
			move_speed *= Mathf.Pow(0.90f, Time.deltaTime/0.016f);
			if(Mathf.Abs(move_speed) < 0.001f){
				move_speed = 0.0f;
			}
			transform.Translate (0, move_speed * Time.deltaTime, 0);
		}
	}
}
