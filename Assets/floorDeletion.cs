using UnityEngine;
using System.Collections;

public class floorDeletion : MonoBehaviour {

	//references
	private static GameObject globalGameManagerObj = null;
	private static GlobalGameManager globalGameManager = null;
	//private GameObject theRoom = null;

	// Use this for initialization

	void Start () {
		globalGameManagerObj = GameObject.Find("GameManager");
		if (globalGameManagerObj == null)
			Debug.Log("Unable to find GameManager for " + this + ".");

		globalGameManager = globalGameManagerObj.GetComponent<GlobalGameManager>();
		if (globalGameManagerObj == null)
			Debug.Log("Unable to find globalGameManager for " + this + ".");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (0) &&
		    globalGameManagerObj.GetComponent<Constructor> ().SelectedTool == Constructor.ToolType.Destroy) 
			{
				Vector3 underTarget = new Vector3 (this.transform.position.x, this.transform.position.y + 10, this.transform.position.z);
				//int size = (int)this.GetComponent<Room> ().RoomSize;
				int size = 1;
				int midSpot = (size / 2) + 1;
				for (int i = 1; i <= size; i++) 
				{
					Vector3 underTargetTemp;

					if (i < midSpot) {
						underTargetTemp = new Vector3 (underTarget.x - (i * 4), underTarget.y, underTarget.z);
					} else if (i == midSpot) {
						underTargetTemp = underTarget;
					} else { // i > midSpot
						underTargetTemp = new Vector3 (underTarget.x + (i - midSpot) * 4, underTarget.y, underTarget.z);
					}

					//hitColliders = Physics.OverlapSphere(this.transform.position, 1.0f);

					// for debug purpose
					//Debug.LogFormat ("there are" + hitColliders.Length + " fools here!");

					// if there is an item below the block, and there is exactly 1 item in the same spot as the block
					if (!Physics.CheckSphere (underTargetTemp, 0.01f)) 
					{ //|| hitColliders.Length != 1) {

						globalGameManager.GetSoundEffect ("deconstruction_s").Play ();
						Destroy (this.gameObject);					//  Debug.LogFormat("yo, we staying here");
						//return true;
						//blockFollowMouse = !blockFollowMouse;
						//transform.position = new Vector3(transform.position.x, transform.position.y); //return z to 0
					}
				}
			}
		}
}
