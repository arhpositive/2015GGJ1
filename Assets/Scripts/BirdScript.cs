using UnityEngine;
using System.Collections;

public class BirdScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.left * Time.deltaTime * 5.0f);
	}
}
