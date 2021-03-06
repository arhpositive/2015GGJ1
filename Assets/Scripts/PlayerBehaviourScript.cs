﻿using UnityEngine;
using System.Collections;

public class PlayerBehaviourScript : MonoBehaviour {
    GameObject logic;
    public KeyCode playerMoveLeft;
    public KeyCode playerMoveRight;
    public KeyCode playerLaunchBall;
	public int playerCode;
	//public bool reversed = false;
    public const float maxMovementSpeed = 12.0f;
    public const float maxAnimSpeed = 3.0f;
    public const float minAnimSpeed = 1.0f;

	float time_since_move_start;
    bool moving_left, moving_right;
    float move_speed;
	float hit_start;
	bool hitting;
	Vector3 initial_pos;
	
	Quaternion initial_left_rotation;
	Quaternion initial_right_rotation;


	BallBehaviour ballBehaviour;
    Animator animator;

	void Start () {
        logic = GameObject.FindWithTag("Logic");
		initial_pos = transform.position;
		ballBehaviour = GameObject.Find ("BallPrefab").GetComponent<BallBehaviour> ();
		initial_left_rotation = transform.FindChild ("left_arm").rotation;
		initial_right_rotation = transform.FindChild ("right_arm").rotation;
        animator = gameObject.GetComponent<Animator>();
		reset ();
	}

	public void reset(){
		transform.FindChild ("left_arm").rotation = initial_left_rotation;
		transform.FindChild ("right_arm").rotation = initial_right_rotation;

		transform.position = initial_pos;
		moving_left = false;
		moving_right = false;
		move_speed = 0.0f;
		hit_start = 0.0f;
		hitting = false;
		time_since_move_start = 0.0f;
	}

	
	public void StartHitAnim() {
		hitting = true;
		hit_start = 0.0f;
	}

	void Update () {
		if(logic.GetComponent<LogicScript>().GameIsOn()){
	        if (Input.GetKey(playerMoveLeft))
	        {
	            if (!moving_left)
	            {
	                moving_left = true;
	                time_since_move_start = 0.0f;
	            }
	        }
	        else
	        {
	            moving_left = false;
	        }

	        if (Input.GetKey(playerMoveRight))
	        {
	            if (!moving_right)
	            {
	                moving_right = true;
	                time_since_move_start = 0.0f;
	            }
	        }
	        else
	        {
	            moving_right = false;
	        }

	        if (Input.GetKeyDown(playerLaunchBall))
	        {
				if(logic.GetComponent<LogicScript>().getCurrentPlayer() == playerCode){
					logic.GetComponent<LogicScript>().StopBar();
				}
	        }

			if (moving_left && moving_right) 
	        {
				time_since_move_start = 0.0f; 
			}
	        else
	        {
	            if (moving_left || moving_right) 
	            {
	                time_since_move_start += Time.deltaTime;
				    move_speed = Mathf.Clamp01 (time_since_move_start / 0.5f) * 30.0f;
				    if (moving_left) 
	                {
				    		move_speed = -move_speed;
				    }
	            }
	            else
	            {
	                move_speed *= Mathf.Pow(0.90f, Time.deltaTime / 0.016f);
				    if (Mathf.Abs(move_speed) < 0.001f)
	                {
				    	move_speed = 0.0f;
				    }
	            }

	            transform.Translate (move_speed * Time.deltaTime, 0, 0);
	        }

			float x = transform.position.x;
			if (Mathf.Abs (x) > maxMovementSpeed) 
	        {
				x = Mathf.Clamp(x, -maxMovementSpeed, maxMovementSpeed);
				transform.position = new Vector3(x, transform.position.y, transform.position.z);
			}
			animator.SetFloat("charSpeed", move_speed);

			bool reversed = playerCode != 1;

	        float a = (reversed ? -1.0f : 1.0f) * Mathf.Clamp (x / 10.0f, -1.0f, 1.0f);
	        a = Mathf.Sign (a) * Mathf.Pow (Mathf.Abs (a), 1.0f);
			
	        float left_angle, right_angle;
	        if (a < 0) 
	        {
	            left_angle = a * 5.0f;
	            right_angle = a * 15.0f;
	        } 
	        else 
	        {
	            left_angle = a * 15.0f;
	            right_angle = a * 5.0f;
	        }
	        Quaternion hit_quaternion = Quaternion.identity;
	        if (hitting) 
	        {
	            hit_start += Time.deltaTime*8;
	            if(hit_start >= 2.0f)
	            {
	                hitting = false;
	            }
	            else
	            {
	                float ang = 0.0f;
	                if (hit_start < 1.0f)
	                {
	                    ang = (reversed?-1.0f:1.0f)*30.0f * (hit_start);
	                }
	                else
	                {
	                    ang = (reversed?-1.0f:1.0f)*30.0f * (2.0f-hit_start);
	                }
	                hit_quaternion = Quaternion.AngleAxis(ang, Vector3.left);
	            }
	        }
	        
	        transform.FindChild ("left_arm").rotation = hit_quaternion * Quaternion.AngleAxis(left_angle, Vector3.up) * initial_left_rotation;
			transform.FindChild ("right_arm").rotation = hit_quaternion * Quaternion.AngleAxis(right_angle, Vector3.up) * initial_right_rotation;
		
		}
	}
}
