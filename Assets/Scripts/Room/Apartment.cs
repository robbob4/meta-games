// -------------------------------- Apartment.cs ------------------------------
// Author - Robert Griswold CSS 385
// Created - May 18, 2016
// Modified - May 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a apartment room that inherits from the leased
// class.
// ----------------------------------------------------------------------------
// Notes - None
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Apartment : Leased
{
    // Use this for fast initialization
    void Awake()
    {
		theInterest = Patron.Interest.Home;
        roomSize = Room.Size.Small;
        constructionCost = 25000;
        maint = 250;
        rent = 10000;
        capacity = 1;
        desc = "Apartments have a for rent and leased state. When leased, a patron will visit daily. Otherwise a patron during business hours may lease it.";
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
