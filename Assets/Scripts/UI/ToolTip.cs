// ---------------------------- ToolTip.cs ----------------------------------
// Author - Samuel Williams
// Created - May 25, 2016
// Modified - May 25, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a UI description box used in conjunction with 
// the ButtonController to display information about the rooms.
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public class ToolTip : MonoBehaviour {

	#region Variables

	//references
	private GameObject roomStatsPanel = null;
	private Text roomName = null;
	private Text roomProfit = null;
	private Text roomHappiness = null;
	private Text roomCapacity = null;
	private Vector3 target = new Vector3(0, 0, 0);
	private GameObject[] tipElements = null;


	//private Text descText = null;

	NumberFormatInfo format = null;
	#endregion

	void Awake()
	{
		tipElements = GameObject.FindGameObjectsWithTag ("ToolTip");
	}

	// Use this for initialization
	void Start ()
	{
		roomStatsPanel = GameObject.Find("ToolTip");
		if (roomStatsPanel == null)
			Debug.Log("Unable to find ToolTip for " + this + ".");

		roomName = GameObject.Find("RoomName").GetComponent<Text>();
		if (roomName == null)
			Debug.Log("Unable to find RoomCapacity for " + this + ".");

		roomProfit = GameObject.Find("RoomProfit").GetComponentsInChildren<Text>()[1];
		if (roomProfit == null)
			Debug.Log("Unable to find RoomCost for " + this + ".");

		roomHappiness = GameObject.Find("RoomHappiness").GetComponentsInChildren<Text>()[1];
		if (roomHappiness == null)
			Debug.Log("Unable to find RoomUpkeep for " + this + ".");
		
		roomCapacity = GameObject.Find("RoomCapacity").GetComponentsInChildren<Text>()[1];
		if (roomCapacity == null)
			Debug.Log("Unable to find RoomUpkeep for " + this + ".");

		format = new NumberFormatInfo();
		format.CurrencyDecimalDigits = 0; //remove decminal from the number format

		//roomStatsPanel.SetActive(false); //hide the panel
		roomStatsPanel.GetComponent<Image>().enabled= false;

		for (int i = 0; i < tipElements.Length; i++) {
			tipElements[i].SetActive(false);
		}

	}

	// Update is called once per frame
	void Update ()
	{
		calculateTarget();
		this.transform.position = target;
	}

	public void SetName(string title)
	{
		roomName.text = title;
		roomStatsPanel.SetActive(true);
	}

	public void SetProfit(int cost)
	{

		roomProfit.text = cost.ToString("c", format);
	}

	public void setHappiness(int upkeep)
	{
		roomHappiness.text = upkeep.ToString("c", format);
	}

	public void SetCapacity(string desc)
	{
		roomCapacity.text = desc;
	}

	public void SelectNone()
	{
		roomStatsPanel.SetActive(false);
	}

	void calculateTarget()
	{
		//current mouse position
		Vector3 currMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		target = currMousePosition;
		target.x -= 80;
		target.y += (74/2);
	}

	public GameObject[] getObjs()
	{
		return this.tipElements;
	}

}