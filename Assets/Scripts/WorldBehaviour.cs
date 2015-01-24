using UnityEngine;
using System.Collections;

public class WorldBehaviour : MonoBehaviour {


	void Start(){

	}

	void Update () {
		this.transform.rotation = this.transform.rotation * Quaternion.AngleAxis(-Time.deltaTime*10.0f, Vector3.left);
	}
}
