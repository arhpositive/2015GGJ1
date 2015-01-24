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
        mainCamera.SetActive(false);
        uiCamera.SetActive(true);
    }
}
