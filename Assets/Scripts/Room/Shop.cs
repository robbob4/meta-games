// -------------------------------- Shop.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 2, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a shop room that inherits from the room class.
// ----------------------------------------------------------------------------
// Notes - Medium size.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Shop : Room
{
	// Use this for initialization
	void Awake ()
    {
        _roomSize = Room.Size.Medium;
        _prefabLocation = "Prefabs/Room/Floor"; //TODO: Update this prefab
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
