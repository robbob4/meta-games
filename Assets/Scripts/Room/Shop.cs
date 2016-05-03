// -------------------------------- Shop.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 3, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a shop room that inherits from the room class.
// ----------------------------------------------------------------------------
// Notes - Medium size.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Shop : Room
{
	// Use this for fast initialization
	void Awake ()
    {
        _roomSize = Room.Size.Medium;
        _prefabLocation = "Prefabs/Room/Floor"; //TODO: Update this prefab
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Deconstruction
    //announcment that the room is about to be destroyed
    public override void Evict()
    {

    }
    #endregion
}
