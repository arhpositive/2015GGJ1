using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
    GameObject leftGuy;
    GameObject rightGuy;
	public Vector3 initial_pos;
    GameObject mainCamera;
	GameObject logic;

	void OnTriggerExit(Collider other) {
		if (other.tag == "sceneBounds") {
			transform.position = initial_pos;
			rigidbody.velocity = Vector3.zero;
			logic.GetComponent <LogicScript> ().OnDeath();
		}
	}
    
	void Start () {
		initial_pos = transform.position;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		leftGuy = GameObject.FindGameObjectWithTag("leftGuy");
		rightGuy = GameObject.FindGameObjectWithTag("rightGuy");
        logic = GameObject.FindGameObjectWithTag("Logic");
	}

	// Update is called once per frame 
	void Update () {
        bool gameIsOn = logic.GetComponent<LogicScript>().GameIsOn();
        if (gameIsOn) {
            Camera mainCamera = Camera.main;
            if (Vector3.Cross(rigidbody.velocity, Vector3.up).sqrMagnitude != 0.0f) {
                Quaternion target_quat = Quaternion.LookRotation(rigidbody.velocity, Vector3.up);

                float angle_diff = Quaternion.Angle(mainCamera.transform.rotation, target_quat);
                float max_angle_rot = Time.deltaTime * 360;

                if (angle_diff < max_angle_rot)
                {
                    mainCamera.transform.rotation = target_quat;
                }
                else
                {
                    mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, target_quat, max_angle_rot / angle_diff);
                }
            }
            mainCamera.transform.position = transform.position + mainCamera.transform.rotation * Vector3.forward * -3.5f + Vector3.up * 1.5f;
        }
	}

    public void ResetBall()
    {
        gameObject.layer = 8; //reset to leftGuy;

        foreach (Transform tr in leftGuy.transform)
        {
            if (tr.gameObject.CompareTag("rightArm") == true)
            {
                //set position of ball slightly above arm
                BoxCollider boxCollider = (BoxCollider)tr.gameObject.collider;
                Vector3 colliderWorldPos = tr.TransformPoint(boxCollider.center);
                transform.position = colliderWorldPos + Vector3.up * 5.0f;
            }
        }
        rigidbody.velocity = Vector3.zero;
    }
}
