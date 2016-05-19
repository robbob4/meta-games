// -------------------------------- Shop.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a shop room that inherits from the retail
// class.
// ----------------------------------------------------------------------------
// Notes - Randomly selects an associated Patron interest.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Shop : Retail
{
    // Use this for fast initialization
    void Awake()
    {
        initReferences();
        roomSize = Room.Size.Small; //medium?
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
        theInterest = (Patron.Interest)Mathf.RoundToInt(Random.Range(-0.5f, 3.4f));
    }

    // Update is called once per frame
    void Update()
    {
        spawner();
    }
}