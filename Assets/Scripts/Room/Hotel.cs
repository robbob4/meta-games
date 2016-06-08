// -------------------------------- Hotel.cs ----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a single hotel room that inherits from the
// leased class.
// ----------------------------------------------------------------------------
// Notes - None
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Hotel : Leased
{
    // Use this for fast initialization
    void Awake()
    {
		theInterest = Patron.Interest.Hotel;
		roomSize = Room.Size.Medium;
        constructionCost = 50000;
        maint = 500;
        rent = 5000;
        capacity = 1;
        windowShopping = false; //only allow the spawned patron to visit
        desc = "Hotel rooms may attract a patron to your tower during business hours to stay the night.";
        name = "Medium Hotel Room"; //Sets the name of the room
        evictHour = 11;
		evictPM = false;
		minHour = 12;
		maxHour = 24;
    }

    // Use this for initialization
    void Start()
    {
        visitors = new Patron[capacity];
        rentors = new GameObject[capacity];
    }

    // Update is called once per frame
    void Update()
    {
		maintainance ();
		//Debug.Log ("hotel update: spawns" + spawnCount + " cap: " + capacity);
		if (spawnCount < capacity)
		{
			if (spawner () == true)
			{
				spawnCount++;
			}
		}
    }

    //override maint and evict to delay visit and traffic changes
    protected override void maintainance()
    {
        int oldVisits = Visits;
        int oldTraffic = Traffic;
        bool previousMaint = maintainanceDeducted;
        base.maintainance();
        if (!previousMaint && previousMaint != maintainanceDeducted)
        {
            Visits = oldVisits;
            Traffic = oldTraffic;
        }
    }

    public override void Evict()
    {
        base.Evict();
        if (Visits == 0)
            Traffic -= 10;
        else
            Visits = 0;
    }
}
