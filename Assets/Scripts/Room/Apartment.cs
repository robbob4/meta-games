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
        spawnChance = 2;
        windowShopping = false; //only allow the spawned patron to visit
        desc = "Apartments have a for rent and leased state. When leased, a patron will visit daily. Otherwise a patron during business hours may lease it.";
        name = "Apartment"; //Sets the name of the room
        evictHour = 5;
        evictPM = false;
        minHour = 14;
        maxHour = 17;
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
            if (!rented)
            {
                if (spawner() == true)
                {
                    spawnCount++;
                    rented = true;
                }
            }
            else
            {
                if (spawner(true) == true)
                {
                    spawnCount++;
                }
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
        if (gameTimer.Hour == 0 && !gameTimer.PM && !previousMaint)
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
