using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
    BoxCollider collider;
    public float sideRatio;
    public float distortFactor;

	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) {
        Rigidbody body = collision.gameObject.rigidbody;
        if (body == null || body.isKinematic)
            return;

        // Compute the initial velocity vector.
        ContactPoint contact = collision.contacts[0];
        /*
         * Logic below assumes edges of the box collider is parallel to axes.
         */
        Vector3 colliderSize = Vector3.Scale(transform.localScale, collider.size);
        Vector3 colliderTopCenter;
        colliderTopCenter = transform.position + collider.center;
        colliderTopCenter.y = colliderSize.y / 2;
        
        float distFromHandCenter = (contact.point.x - colliderTopCenter.x);
        float finalX = contact.point.x + distFromHandCenter * distortFactor;
        float planeZSize = 26f;  // TODO: Retrieve these.
        float finalZ = (planeZSize == contact.point.z ? 0f : planeZSize);

        

        // Push the ball.
        //Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        Vector3 destPoint = new Vector3(finalX, body.position.y, finalZ);
        Vector3 pushDir = new Vector3(finalX, 0, finalZ);
        pushDir.Normalize();
        pushDir.y = pushDir.magnitude;
        pushDir.Normalize();
        float zDist = Mathf.Abs(finalZ - contact.point.z);
        float numerator = zDist * zDist * Physics.gravity.magnitude;
        float denominator = (pushDir.z * pushDir.z * pushDir.y * pushDir.y * body.mass);

        float pushPower = 1.0F;
        //pushPower = Mathf.Sqrt(Mathf.Sqrt( numerator / denominator ));

        float dist, voPlanar, vox, voy, voz, duration = 2.0f;
        vox = Mathf.Abs(destPoint.x - body.position.x) / duration;
        voz = Mathf.Abs(destPoint.z - body.position.z) / duration;
        voy = (0.5f * duration * duration * Physics.gravity.magnitude) / duration;
        pushDir = new Vector3(vox, voy, voz);

        body.velocity = pushDir * pushPower;
    }
}

