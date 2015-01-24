using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
	GameObject camera;
    GameObject leftGuy;
    GameObject rightGuy;
	public Vector3 initial_pos;
	GameObject myCamera, uiCamera;
    bool gameOn;

	void OnTriggerExit(Collider other) {
		if (other.tag == "sceneBounds") {
			transform.position = initial_pos;
			rigidbody.velocity = Vector3.zero;
			reset ();
			leftGuy.GetComponent <PlayerBehaviourScript> ().reset();
			rightGuy.GetComponent <PlayerBehaviourScript> ().reset();
		}
	}
    
	void Start () {
		initial_pos = transform.position;
		camera = GameObject.Find ("Main Camera");
		leftGuy = GameObject.FindGameObjectWithTag("leftGuy");
		rightGuy = GameObject.FindGameObjectWithTag("rightGuy");

		// Switch to UI Camera, initiate game mode as off
		myCamera = GameObject.FindWithTag("MainCamera");
		uiCamera = GameObject.FindWithTag("UICamera");
		reset ();
	}

	public void reset(){
        ResetBall();
	    myCamera.transform.rotation = Quaternion.LookRotation (-Vector3.up, Vector3.forward);

        // Switch to UI Camera, game mode off
        //rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        gameOn = false;
        GameObject playButton = GameObject.FindWithTag("UI_PlayButton");
        //playButton.guiText.text = "Replay";
        myCamera.SetActive(false);
		uiCamera.SetActive(true);

	}

    public void InitiateBouncing()
    {
        ResetBall();
        uiCamera.SetActive(false);
        myCamera.SetActive(true);
        gameOn = true;
//      rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
    }
	public bool IsGameOn(){
		return gameOn;
	}
	// Update is called once per frame 
	void Update () {
        if (gameOn) {
            if (Vector3.Cross(rigidbody.velocity, Vector3.up).sqrMagnitude != 0.0f) {
                Quaternion target_quat = Quaternion.LookRotation(rigidbody.velocity, Vector3.up);

                float angle_diff = Quaternion.Angle(myCamera.transform.rotation, target_quat);
                float max_angle_rot = Time.deltaTime * 360;

                if (angle_diff < max_angle_rot)
                {
                    myCamera.transform.rotation = target_quat;
                }
                else
                {
                    myCamera.transform.rotation = Quaternion.Lerp(myCamera.transform.rotation, target_quat, max_angle_rot / angle_diff);
                }
            }
            myCamera.transform.position = transform.position + myCamera.transform.rotation * Vector3.forward * -3.5f + Vector3.up * 1.5f;
        }
        else {
//          rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
	}

    void ResetBall()
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
