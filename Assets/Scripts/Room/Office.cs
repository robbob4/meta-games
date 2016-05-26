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
        roomSize = Room.Size.Medium;
        constructionCost = 100000;
        maint = 1000;
        rent = 30000;
        capacity = 1;
        desc = "Offices have a for rent and leased state. When leased, a patron will visit daily. Otherwise a patron during business hours may lease it.";
    }

    // Use this for initialization
    void Start()
    {
        visitors = new Patron[capacity];
    }

    // Update is called once per frame
    void Update()
    {
        //while (spawnCount < capacity)
        //{
        //    spawner();
        //}
    }
}
