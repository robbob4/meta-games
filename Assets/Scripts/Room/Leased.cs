// ------------------------------- Leased.cs ----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base leased room that inherits from the room
// class.
// ----------------------------------------------------------------------------
// Notes - Reduced spawn chance to 3%.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Leased : Room
{
    #region Variables
    protected int spawnCount = 0;
    protected bool rented = false;
    #endregion

    public Leased() //constructor - only non-unity things here
    {
        spawnChance = 3;
    }
}
