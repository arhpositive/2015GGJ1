﻿using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
    BoxCollider boxCollider;
    public float distortFactor;
    public static float zDistFromCenter = 26.0f;
    public static float startDuration = 6.0f;
    public static float accelerationPace = 0.05f;
    public static int scoreBaseAdditionForHit = 50;
    public static float scoreBaseMultiplierForSpeed = 250.0f;
    public static int scoreBaseAdditionForBarAccuracy = 25;

    LogicScript logicScript;
	GameObject logic;
    public static float duration;

	// Use this for initialization
	void Start () {
        boxCollider = GetComponent<BoxCollider>();
		duration = startDuration;
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
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

        //HS1: add speed scores
        int veloscore = (int)(scoreBaseMultiplierForSpeed / duration);
        print("velo score: " + veloscore);
        logicScript.AddToHighScore(veloscore);

        if (!logicScript.isBarMoving())
        {
            logicScript.SwapPlayer();

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

            //HS2: add hand accuracy scores
            int accuscore = scoreBaseAdditionForHit - (int)(Mathf.Abs(distFromHandCenter * distortFactor));
            print("accu score: " + accuscore);
            logicScript.AddToHighScore(accuscore); //TODO ARHAN change

            //HS3: add hitbar accuracy scores
            int hitbarscore = scoreBaseAdditionForBarAccuracy + (int)(scoreBaseAdditionForHit * (1.0f - logicScript.getBarValue()));
            print("hitbar score: " + hitbarscore);
            logicScript.AddToHighScore(hitbarscore);

            Vector3 pushDir = new Vector3(vox, voy, voz);
            body.velocity = pushDir;
        }       
    }

    public void ResetDuration() {
        duration = startDuration;
    }
}