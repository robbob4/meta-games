// ----------------------------- Restaurant.cs --------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a restaurant room that inherits from the retail
// class.
// ----------------------------------------------------------------------------
// Notes - Randomly selects an associated Patron interest.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Restaurant : Retail
{
    // Use this for fast initialization
    void Awake()
    {
        roomSize = Room.Size.Large;
        capacity = 20;
        constructionCost = 150000;
        maint = 1500;
        rent = 1000;
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