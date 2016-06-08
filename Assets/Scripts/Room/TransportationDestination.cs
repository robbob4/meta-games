// -------------------------------- TransportationDestination.cs -----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a base room class that inherits from the 
// Destination class. Specifies traffic, spawnchance, and spawnDelayModulo.
// spawnDelayModulo just specifies what minute to attempt to spawn.
// ----------------------------------------------------------------------------
// Notes - Child must call spawner() if desired in update.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class TransportationDestination : Destination
{
    #region Variables
    protected bool[] servicedFloors;
    #endregion

    // Use this for fast initialization
    void Awake()
    {
        servicedFloors = new bool[Pathing.MAX_FLOORS_HIGH];
        transportation = true;
        servicedFloors[Floor] = true;
    }

    #region Getters and Setters
    public bool FloorService(int query)
    {
        return servicedFloors[query];
    }
    #endregion
}
