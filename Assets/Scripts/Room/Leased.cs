// ------------------------------- Leased.cs ----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 12, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base leased room that inherits from the room
// class.
// ----------------------------------------------------------------------------
// Notes - Reduced spawn chance to 5%.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Leased : Room
{
    // Use this for fast initialization
    void Awake()
    {
        spawnChance = 5;
    }

    // Use this for initialization
    void Start()
    {
        visitors = new Patron[capacity];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
