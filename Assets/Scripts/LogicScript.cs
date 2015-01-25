using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    static int numLives = 3;
    static int comboMultiLimit = 10;
    GameObject mainCamera, uiCamera, helpCamera, creditCamera;
    GameObject leftGuy, rightGuy;
    GameObject ballOfSteel;
    BallBehaviour ballBehaviourScript;
    bool gameOn;
    int remainingLives;
    int playerHighScore;
    int gameHighScore;
    int comboMultiplier;
	
	GameObject bar;
	float max_bar_move;
	float bar_value;
	bool bar_moving = false;
	int current_player;
	Text scoreText;
	Text livesText;
	Text highScoreText;
	Text comboText;
	Text messageText;
	Text currentPlayerText;
	bool hasMessage = false;
	float messageTime = 0.0f;
	float comboTime = 0.0f;

	public Color color0, color1;

    // Use this for initialization
    void Start()
    {
		// Switch to UI Camera, initiate game mode as off
        ballOfSteel = GameObject.FindGameObjectWithTag("BallOfSteel");
        ballOfSteel.rigidbody.useGravity = false;
        ballBehaviourScript = ballOfSteel.GetComponent<BallBehaviour>();

        mainCamera = GameObject.FindWithTag("MainCamera");
        uiCamera = GameObject.FindWithTag("UICamera");
        helpCamera = GameObject.FindWithTag("HelpCamera");
        creditCamera = GameObject.FindWithTag("CreditCamera");

	    mainCamera.transform.rotation = Quaternion.LookRotation (-Vector3.up, Vector3.forward);
        remainingLives = numLives;
        SwitchToUICamera();

		leftGuy = GameObject.FindGameObjectWithTag("leftGuy");
		rightGuy = GameObject.FindGameObjectWithTag("rightGuy");
		
		scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text> ();
		livesText = GameObject.FindGameObjectWithTag("LivesText").GetComponent<Text> ();
		highScoreText = GameObject.FindGameObjectWithTag("HiScoreText").GetComponent<Text> ();
		comboText = GameObject.Find ("comboText").GetComponent<Text> ();
		messageText = GameObject.Find ("messageText").GetComponent<Text> ();
		currentPlayerText = GameObject.Find ("currentPlayerText").GetComponent<Text> ();
		

		bar = GameObject.Find ("bar_arrow");
		max_bar_move = 90.0f;
		
        gameHighScore = 0;
        playerHighScore = 0;
        comboMultiplier = 1;
		highScoreText.text = "High Score:" + 0;
		currentPlayerText.text = "Left";
    }

	public void SwapPlayer(){
		bar_moving = true;
		if (current_player == 1) {
			current_player = 2;
			currentPlayerText.text = "Right";
		} else {
			current_player = 1;
			currentPlayerText.text = "Left";
		}
	}
	public int getCurrentPlayer(){
		return current_player;
	}
	public void StopBar(){
        if (bar_moving)
        {
            bar_moving = false;
            print(bar_value);
        }		
	}
    public void OnDeath()
    {
		--remainingLives;
		livesText.text = "Lives:" + remainingLives.ToString();
		mainCamera.transform.rotation = Quaternion.LookRotation(-Vector3.up, Vector3.forward);
        ResetPlayerPositions(); // Must be called before resetBall
		current_player = 1;
		currentPlayerText.text = "Left";
        ballBehaviourScript.ResetBall();
        ResetHandSpeeds();
		comboMultiplier = 0;
		comboText.text = "";
        if (remainingLives <= 0)
        {
            remainingLives = 3;
            ballOfSteel.rigidbody.useGravity = false;
            gameOn = false;
            if (gameHighScore < playerHighScore)
            {
                gameHighScore = playerHighScore;
                highScoreText.text = "High Score:" + gameHighScore;
            }
            SwitchToUICamera();
        }
        else
        {
            bar_moving = false;
			bar_value = 0.0f;
			
			bar.GetComponent<RectTransform> ().localPosition = new Vector3 (0, 0, 0);
        }
    }

    public bool isBarMoving()
    {
        return bar_moving;
    }

    public bool GameIsOn()
    {
        return gameOn;
    }

    public float getBarValue()
    {
        return bar_value;
    }

    public void increaseComboMultiplier()
    {
        if (comboMultiplier < comboMultiLimit)
        {
            comboMultiplier++;
            print("Combo at " + comboMultiplier + "x!");
		}
		if (comboMultiplier == 1) {
			return;
		}

		comboText.text = "Combo x"+comboMultiplier+" !";
    }

    public void resetComboMultiplier()
    {
        comboMultiplier = 1;
		comboText.text = "";
    }

    public void InitiateBouncing()
	{
		bar_moving = false;
		playerHighScore = 0;
		comboMultiplier = 0;
		scoreText.text = "Score:" + playerHighScore;
		livesText.text = "Lives:" + 3;
		comboText.text = "";
		hasMessage = false;
		messageText.text = "";
		current_player = 1;
        ballBehaviourScript.ResetBall();
        ResetHandSpeeds();
        switchToMainCamera();
        gameOn = true;
        ballOfSteel.rigidbody.useGravity = true;
    }
	
	// Update is called once per frame
	void Update () {
		float interpolate = Mathf.Clamp01(ballOfSteel.transform.position.y/50.0f);
		interpolate = Mathf.Pow (interpolate, 1.2f);
		//mainCamera.GetComponent<Camera> ().backgroundColor = Color.Lerp(color0, color1, interpolate);
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
            mainCamera.transform.position = ballOfSteel.transform.position + mainCamera.transform.rotation * Vector3.forward * -2.5f + Vector3.up * 1.75f;
        	
			if (bar_moving) {
				float bar_move = Mathf.Sin (Time.time * 6.0f);
				bar.GetComponent<RectTransform> ().localPosition = new Vector3 (0, bar_move * max_bar_move, 0);
				bar_value = Mathf.Abs (bar_move);
			}

			if(hasMessage){
				messageTime += Time.deltaTime;
			}
			if(messageTime > 2.0f){
				hasMessage = false;
				messageText.text = "";
			}
			if(hasMessage){
				float a = 0.0f;
				if(messageTime < 0.4f){
					a = messageTime/0.4f;
				}else if (messageTime < 1.8f){
					a = 1.0f;
				}else{
					a = (2.0f-messageTime)/0.2f;
				}
				
				Color c = messageText.color;
				c.a = a;
				messageText.color = c;
			}

		}
	}

    public void SwitchToUICamera()
    {
        // Switch to UI Camera, game mode off
		helpCamera.SetActive(false);
        mainCamera.SetActive(false);
		creditCamera.SetActive(false);
		uiCamera.SetActive(true);
		GameObject.FindWithTag ("GameUI").GetComponent<Canvas>().enabled = false;
    }

    void switchToMainCamera()
	{
        // Switch to UI Camera, game mode off
		helpCamera.SetActive(false);
        uiCamera.SetActive(false);
		creditCamera.SetActive(false);
		mainCamera.SetActive(true);
		GameObject.FindWithTag ("GameUI").GetComponent<Canvas>().enabled = true;
    }

    public void ResetHandSpeeds() {
        GameObject.FindGameObjectWithTag("rightArm").GetComponent<Hand>().ResetDuration();
        string[] armTags = {"leftArm", "rightArm"};
        foreach(string s in armTags)
        {
            foreach(GameObject go in GameObject.FindGameObjectsWithTag(s))
            {
                go.GetComponent<Hand>().ResetDuration();
            }
        }
    }

    public void ResetPlayerPositions() {
        leftGuy.GetComponent<PlayerBehaviourScript>().reset();
        rightGuy.GetComponent<PlayerBehaviourScript>().reset();
    }

    public void AddToHighScore(int addition)
    {
        playerHighScore += addition * comboMultiplier;
		scoreText.text = "Score:" + playerHighScore;
        print(playerHighScore);
    }
	
	public void DisplayMessage(string message)
	{
		messageText.text = message;
		hasMessage = true;
		messageTime = 0.0f;

	}

    public void SwitchToCreditCamera()
    {
		mainCamera.SetActive(false);
		uiCamera.SetActive(false);
		helpCamera.SetActive(false);
		creditCamera.SetActive(true);
		GameObject.FindWithTag ("GameUI").GetComponent<Canvas>().enabled = false;
    }

    public void SwitchToHelpCamera()
    {
		mainCamera.SetActive(false);
		uiCamera.SetActive(false);
		creditCamera.SetActive(false);
		helpCamera.SetActive(true);
		GameObject.FindWithTag ("GameUI").GetComponent<Canvas>().enabled = false;
    }
}
