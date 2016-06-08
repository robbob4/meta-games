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
		theInterest = Patron.Interest.Office;
        roomSize = Room.Size.Medium;
        constructionCost = 100000;
        maint = 1000;
        rent = 10000;
        capacity = 3;
        spawnChance = 1;
        windowShopping = false; //only allow the spawned patron to visit
        desc = "Offices have a for rent and leased state. When leased, some patrons will visit daily. Otherwise a patron during business hours may lease it.";
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
        maintainance();
        if (spawnCount < capacity)
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

    public override bool Visit(Patron visitor)
    {
        bool retVal = base.Visit(visitor);
        Traffic -= 10; //negate traffic per visitor

        if(currentCapacity == MaxCapacity)
            Traffic += 10; //grant traffic when all patrons can visit

        return retVal;
    }
}
