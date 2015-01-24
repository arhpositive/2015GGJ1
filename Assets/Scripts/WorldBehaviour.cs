using UnityEngine;
using System.Collections;

public class WorldBehaviour : MonoBehaviour {




	void Update () {
		this.transform.rotation = this.transform.rotation * Quaternion.AngleAxis(Time.deltaTime, Vector3.left);
	}
}
