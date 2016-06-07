// -------------------------------- Office.cs ---------------------------------
// Author - Robert Griswold CSS 385
// Created - May 18, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a office room that inherits from the leased
// class.
// ----------------------------------------------------------------------------
// Notes - None
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Office : Leased
{
    // Use this for fast initialization
    void Awake()
    {
		theInterest = Patron.Interest.Home;
        roomSize = Room.Size.Medium;
        constructionCost = 100000;
        maint = 1000;
        rent = 30000;
        capacity = 1;
        desc = "Offices have a for rent and leased state. When leased, a patron will visit daily. Otherwise a patron during business hours may lease it.";
        name = "Office"; //Sets the name of the room
    }

    // Use this for initialization
    void Start()
    {
        visitors = new Patron[capacity];
    }

    // Update is called once per frame
    void Update()
    {
		maintainance ();
		if(spawnCount < capacity)
		{
			if (spawner () == true) {
				spawnCount++;
			}
		}
    }

}
