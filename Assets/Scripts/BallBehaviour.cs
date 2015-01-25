using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
    GameObject leftGuy;
    GameObject rightGuy;
    LogicScript logicScript;
    Component[] transforms;
    Quaternion currentRot;
    Vector3 rotateAxis;
    Quaternion goalRot;
    public static float rotSpeed = 0.1f;
    bool isRotating;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "sceneBounds") {
			rigidbody.velocity = Vector3.zero;
            logicScript.OnDeath();
		}
	}
    
	void Start () {
		leftGuy = GameObject.FindGameObjectWithTag("leftGuy");
		rightGuy = GameObject.FindGameObjectWithTag("rightGuy");
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        currentRot = Quaternion.identity;
        goalRot = Quaternion.identity;
        isRotating = false;
        rotateAxis = Vector3.zero;
	}

	// Update is called once per frame 
	void Update () {
        if (isRotating)
        {
            transform.Rotate(rotateAxis * Time.deltaTime, Space.World);
        }        
	}

    public void setCurrentAndGoalRots()
    {
        isRotating = true;
        float rotvecx = Random.Range(-1.0f, 1.0f);
        float rotvecy = Random.Range(-1.0f, 1.0f);
        float rotvecz = Random.Range(-1.0f, 1.0f);
        rotateAxis = new Vector3(rotvecx, rotvecy, rotvecz);
        rotateAxis.Normalize();
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    public void ResetBall()
    {
        isRotating = false;
        gameObject.layer = 8; //reset to leftGuy;

        transforms = leftGuy.GetComponentsInChildren<Transform>();
        foreach (Transform tr in transforms)
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
