// ------------------------------ FastFood.cs ---------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a fast food room that inherits from the retail
// class.
// ----------------------------------------------------------------------------
// Notes - Randomly selects an associated Patron interest.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class FastFood : Retail
{
    // Use this for fast initialization
    void Awake()
    {
        initReferences();
		roomSize = Room.Size.Medium;
        capacity = 10;
        constructionCost = 100000;
        maint = 1000;
        rent = 500;
    }

    // Use this for initialization
    void Start()
    {
        visitors = new Patron[capacity];

        //Generate the interest type
        theInterest = (Patron.Interest)Mathf.RoundToInt(Random.Range(3.5f, 6.5f));
    }

    // Update is called once per frame
    void Update()
    {
        spawner();
    }
}
