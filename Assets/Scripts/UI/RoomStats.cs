// ---------------------------- RoomStats.cs ----------------------------------
// Author - Brent Eaves CSS 385
// Created - May 4, 2016
// Modified - May 4, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a UI description box used in conjunction with 
// the ButtonController to display information about the rooms.
// ----------------------------------------------------------------------------
// Notes - (Needs more comments).
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomStats : MonoBehaviour {

    private GameObject titleText = null;

	// Use this for initialization
	void Start ()
    {
        titleText = GameObject.Find("RoomType");
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void Select(string selection)
    {
        titleText.GetComponent<Text>().text = selection;
    }

    public void SelectNone()
    {
        titleText.GetComponent<Text>().text = "Sample";
    }
}