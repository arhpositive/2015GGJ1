using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
    BoxCollider boxCollider;
    public float distortFactor;
    public static float zDistFromCenter = 26.0f;
    public static float startDuration = 2.0f;
    public static float accelerationPace = 0.1f;
    float duration;

	// Use this for initialization
	void Start () {
        boxCollider = GetComponent<BoxCollider>();
        duration = startDuration;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) {
        Rigidbody body = collision.gameObject.rigidbody;
        if (body == null || body.isKinematic)
            return;

        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.layer = 9;
        }
        else if (collision.gameObject.layer == 9)
        {
            collision.gameObject.layer = 8;
        }

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
        float vox, voy, voz; 
        vox = (destPoint.x - body.position.x) / duration;
        voz = (destPoint.z - body.position.z) / duration;
        voy = (0.5f * duration * duration * Physics.gravity.magnitude) / duration;
        duration = (1 - accelerationPace) * duration;

        Vector3 pushDir = new Vector3(vox, voy, voz);
        body.velocity = pushDir;
    }
}