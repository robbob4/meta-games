// ------------------------------- Retail.cs ----------------------------------
// Author - Robert Griswold CSS 385
// Created - May 2, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a shop room that inherits from the room class.
// ----------------------------------------------------------------------------
// Notes - None.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Retail : Room
{
    protected override void maintainance()
    {
        if (gameTimer.Hour == 0 && !gameTimer.PM && !maintainanceDeducted)
        {
            if (Happiness <= 20)
                globalGameManager.NewStatus("A " + this.name + " has a happiness of " + Happiness + ".", true);
        }

        base.maintainance();
    }
}
