using UnityEngine;
using System.Collections;

public class cameraMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + .1f, Camera.main.transform.position.y, Camera.main.transform.position.z);
	}
}
