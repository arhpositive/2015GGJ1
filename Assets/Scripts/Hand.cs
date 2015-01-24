using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
    BoxCollider collider;
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

        // Compute destination point.
        ContactPoint contact = collision.contacts[0];
        /*
         * Logic below assumes edges of the box collider is parallel to axes.
         */
        Vector3 colliderSize = Vector3.Scale(transform.localScale, collider.size);
        Vector3 colliderTopCenter;
        colliderTopCenter = transform.position + collider.center;
        colliderTopCenter.y = colliderSize.y / 2;

        float distFromHandCenter = (contact.point.x - colliderTopCenter.x);
        float zDistFromCenter = 26f;  // TODO: Retrieve this from the base size & position.
        float finalX = contact.point.x + distFromHandCenter * distortFactor;
        float finalZ = (contact.point.z > 0 ? -zDistFromCenter : zDistFromCenter);
        Vector3 destPoint = new Vector3(finalX, body.position.y, finalZ);

        // Compute velocity vector.
        float vox, voy, voz, duration = 2.0f; // TODO: dynamically alter duration based on slider action.
        vox = (destPoint.x - body.position.x) / duration;
        voz = (destPoint.z - body.position.z) / duration;
        voy = (0.5f * duration * duration * Physics.gravity.magnitude) / duration;
        Vector3 pushDir = new Vector3(vox, voy, voz);

        body.velocity = pushDir;
    }
}