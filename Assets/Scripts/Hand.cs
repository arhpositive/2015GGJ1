using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
    BoxCollider boxCollider;
    public float distortFactor;
    public static float zDistFromCenter = 26.0f;

	// Use this for initialization
	void Start () {
        boxCollider = GetComponent<BoxCollider>();
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

        Vector3 colliderSize = Vector3.Scale(transform.localScale, boxCollider.size);
        Vector3 colliderTopCenter;
        colliderTopCenter = transform.position + boxCollider.center;
        colliderTopCenter.y = colliderSize.y / 2;

        float distFromHandCenter = (contact.point.x - colliderTopCenter.x);
        float finalX = contact.point.x + distFromHandCenter * distortFactor;
        float finalZ = (contact.point.z > 0 ? -zDistFromCenter : zDistFromCenter);
        Vector3 destPoint = new Vector3(finalX, body.position.y, finalZ);

        // Compute velocity vector.
        float duration = 2.0f; // TODO: dynamically alter duration based on slider action.
        float vox, voy, voz; 
        vox = (destPoint.x - body.position.x) / duration;
        voz = (destPoint.z - body.position.z) / duration;
        voy = (0.5f * duration * duration * Physics.gravity.magnitude) / duration;

        Vector3 pushDir = new Vector3(vox, voy, voz);
        body.velocity = pushDir;
    }
}