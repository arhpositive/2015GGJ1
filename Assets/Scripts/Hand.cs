using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
    BoxCollider boxCollider;
    public float distortFactor;
    public static float zDistFromCenter = 26.8f;
    public static float startDuration = 4.0f;
    public static float accelerationPace = 0.08f;
    public static int scoreBaseAdditionForHit = 50;
    public static float scoreBaseMultiplierForSpeed = 250.0f;
    public static int scoreBaseAdditionForBarAccuracy = 25;

    float durationLowLimit = 2.0f;

    LogicScript logicScript;
    BallBehaviour ballBehaviourScript;
    public static float duration;

	// Use this for initialization
	void Start () {
        boxCollider = GetComponent<BoxCollider>();
		duration = startDuration;
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        ballBehaviourScript = GameObject.FindGameObjectWithTag("BallOfSteel").GetComponent<BallBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider other) {

		if (other.gameObject.layer != gameObject.layer) {
			return;
		}
		if (other.gameObject.tag == "BallOfSteel") {
			Rigidbody body = other.gameObject.rigidbody;
			if (body == null || body.isKinematic)
				return;
			
			if (other.gameObject.layer == 8)
			{
				other.gameObject.layer = 9;
			}
			else if (other.gameObject.layer == 9)
			{
				other.gameObject.layer = 8;
			}
			
			//HS1: add speed scores
			int veloscore = (int)(scoreBaseMultiplierForSpeed / duration);
			print("velo score: " + veloscore);
			logicScript.AddToHighScore(veloscore);
			
			if (!logicScript.isBarMoving())
			{
				ballBehaviourScript.setCurrentAndGoalRots();
				transform.parent.gameObject.GetComponent<PlayerBehaviourScript>().StartHitAnim();
				logicScript.SwapPlayer();
				
				float skillBarValue = logicScript.getBarValue();
				
				// Compute destination point.
				Vector3 ballPos = other.gameObject.transform.position;

				BoxCollider boxCollider = (BoxCollider)gameObject.collider;
				Vector3 colliderWorldPos = transform.TransformPoint(boxCollider.center);

				float distFromHandCenter = (ballPos.x - colliderWorldPos.x);
				float finalX = ballPos.x + distFromHandCenter * distortFactor;
				/*if (distFromHandCenter < 0.0f)
				{
					finalX -= skillBarValue * distortFactor * 2.0f; 
				}
				else
				{
					finalX += skillBarValue * distortFactor * 2.0f; 
				}*/

				float min_r = 3.0f + logicScript.getBarValue()*3.0f;
				float max_r = 6.0f + logicScript.getBarValue()*5.0f;

				//revert for old
				finalX = Random.Range(min_r, max_r);
				if(Random.Range(0.0f, 1.0f) < 0.5f){
					finalX = -finalX;
				}

				float finalZ = (ballPos.z > 0 ? -zDistFromCenter : zDistFromCenter);
				Vector3 destPoint = new Vector3(finalX, body.position.y, finalZ);
				// Compute velocity vector.
				float vox, voy, voz;
				vox = (destPoint.x - body.position.x) / duration;
				voz = (destPoint.z - body.position.z) / duration;
				voy = (0.5f * duration * duration * Physics.gravity.magnitude) / duration;

				float abs_diff = Mathf.Abs(distFromHandCenter) - 0.2f;
				if(abs_diff < 0){
					abs_diff = 0.0f;
				}
				abs_diff *= 0.5f;
				float total_skill = skillBarValue + abs_diff;

				if (duration > durationLowLimit)
				{
					//adjust duration depending on how successful you were //TODO_ARHAN adjustments needed
					if (total_skill > 0.75f)
					{
						duration = (1 - (accelerationPace * skillBarValue * 2.0f)) * duration;
						print("Terrible shot! Duration adjusted to: " + duration);
						logicScript.DisplayMessage("Terrible Shot!");
						logicScript.resetComboMultiplier();
					}
					else if (total_skill > 0.25f)
					{
						duration = (1 - (accelerationPace * skillBarValue)) * duration;
						print("Poor shot! Duration adjusted to: " + duration);
						logicScript.DisplayMessage("Poor Shot!");
						logicScript.resetComboMultiplier();
					}
					else
					{
						if (total_skill < 0.1f)
						{
							print("Good shot! Duration adjusted to: " + duration);
							logicScript.DisplayMessage("Good Shot!");
						}
						logicScript.increaseComboMultiplier();
					}                
				}            
				
				//HS2: add hand accuracy scores
				int accuscore = scoreBaseAdditionForHit - (int)(Mathf.Abs(distFromHandCenter * distortFactor));
				print("accu score: " + accuscore);
				logicScript.AddToHighScore(accuscore); //TODO ARHAN change
				
				//HS3: add hitbar accuracy scores
				int hitbarscore = scoreBaseAdditionForBarAccuracy + (int)(scoreBaseAdditionForHit * (1.0f - skillBarValue));
				print("hitbar score: " + hitbarscore);
				logicScript.AddToHighScore(hitbarscore);
				
				Vector3 pushDir = new Vector3(vox, voy, voz);
				body.velocity = pushDir;
			}else{
				float randAng = Random.Range(0.0f, 360.0f);
				body.velocity = new Vector3(Mathf.Cos (randAng)*2.0f, 0, Mathf.Sin (randAng)*2.0f);
			}
		}
		
	}
	
	void OnCollisionEnter(Collision collision) {

	}

	
	public void ResetDuration() {
		duration = startDuration;
	}
}