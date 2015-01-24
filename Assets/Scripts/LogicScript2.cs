using UnityEngine;
using System.Collections;
 
public class LogicScript : MonoBehaviour
{
    static int numLives = 3;
    GameObject mainCamera, uiCamera;
    GameObject ballOfSteel;
    bool gameOn;
    int remainingLives;

    // Use this for initialization
    void Start()
    {
        // Switch to UI Camera, initiate game mode as off
        ballOfSteel = GameObject.FindGameObjectWithTag("BallOfSteel");
        ballOfSteel.rigidbody.useGravity = false;

        mainCamera = GameObject.FindWithTag("MainCamera");
        uiCamera = GameObject.FindWithTag("UICamera");

	    mainCamera.transform.rotation = Quaternion.LookRotation (-Vector3.up, Vector3.forward);
        remainingLives = numLives;
        switchToUICamera();
    }

    public void OnDeath()
    {
	    mainCamera.transform.rotation = Quaternion.LookRotation (-Vector3.up, Vector3.forward);
        ballOfSteel.rigidbody.useGravity = false;
        gameOn = false;
        --remainingLives;
        switchToUICamera();
    }

    public bool GameIsOn()
    {
        return gameOn;
    }

    public void InitiateBouncing()
    {
        ballOfSteel.GetComponent<BallBehaviour>().ResetBall();
        switchToMainCamera();
        gameOn = true;
        ballOfSteel.rigidbody.useGravity = true;
    }
	
	// Update is called once per frame
	void Update () {
        Rigidbody rigidbody = ballOfSteel.rigidbody;
        if (gameOn) {
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
            mainCamera.transform.position = ballOfSteel.transform.position + mainCamera.transform.rotation * Vector3.forward * -3.5f + Vector3.up * 1.5f;
        }
	}

    void switchToUICamera()
    {
        // Switch to UI Camera, game mode off
        GameObject playButton = GameObject.FindWithTag("UI_PlayButton");
        //        playButton.guiText.text = "Replay";
        mainCamera.SetActive(false);
        uiCamera.SetActive(true);
    }

    void switchToMainCamera()
    {
        // Switch to UI Camera, game mode off
        uiCamera.SetActive(false);
		mainCamera.SetActive(true);
    }
}
