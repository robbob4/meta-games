﻿// -------------------------------- Hotel.cs ----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 12, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a hotel room that inherits from the leased
// class.
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