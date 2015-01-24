using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
    GameObject leftGuy;
    GameObject rightGuy;
	public Vector3 initial_pos;
	GameObject logic;
    Component[] transforms;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "sceneBounds") {
			transform.position = initial_pos;
			rigidbody.velocity = Vector3.zero;
			logic.GetComponent <LogicScript> ().OnDeath();
		}
	}
    
	void Start () {
		initial_pos = transform.position;
		leftGuy = GameObject.FindGameObjectWithTag("leftGuy");
		rightGuy = GameObject.FindGameObjectWithTag("rightGuy");
        logic = GameObject.FindGameObjectWithTag("Logic");
	}

	// Update is called once per frame 
	void Update () {
	}

    public void ResetBall()
    {
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
