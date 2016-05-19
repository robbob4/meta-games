// ----------------------------- GameTime.cs ----------------------------------
// Author - Brent Eaves CSS 385
// Author - Robert Griswold CSS 385
// Created - May 4, 2016
// Modified - May 18, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for clock timer.
// ----------------------------------------------------------------------------
// Notes - (Needs more comments).
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTime : MonoBehaviour {

    private bool isPaused;
    private float timeRatio = 20.0f; //Game Minutes to RL Seconds
    private float offset = 420; //7am prestart
    private Text timeText;
    private float theTime;
    
    private float stoppedTime;
    private int lastHour;
    private int lastMin;
    private bool pm;

	void Start ()
    {
        isPaused = false;
        timeText = GameObject.Find("Information/World Time").GetComponent<Text>();
        if (timeText == null)
            Debug.Log("Information/World Time not found for " + this + ".");
        theTime = Time.time;
        pm = false;
        stoppedTime = 0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (isPaused)
        {
            stoppedTime += Time.deltaTime;
        }
        else
        {
            theTime = ((Time.time - stoppedTime) * timeRatio) + offset;
        }

        convertTime();
    }

    public void PauseThis (bool toPause)
    {
        isPaused = toPause;
    }

    void convertTime ()
    {
        int hours, mins;
        string sHours, sMins;

        mins = (int) theTime % 60;

        //adjust single digit mins
        if ( mins < 10)
        {
            sMins = "0" + mins;
        }
        else
        {
            sMins = "" + mins;
        }

        hours = ((int)theTime / 60) %12;

        //toggle am/pm
        if (lastHour == 11 && hours == 0)
            pm = !pm;

        lastHour = hours;
        lastMin = mins;

        //0 hours adjustment
        if (hours == 0)
        {
            sHours = "12";
        }
        else
        {
            sHours = "" + hours;
        }

        timeText.text = sHours + ":" + sMins;

        if (pm)
        {
            timeText.text += " PM";
        }
        else
        {
            timeText.text += " AM";
        }
    }

    #region Getters and setters
    public bool PM
    {
        get { return pm; }
    }

    public int Min
    {
        get { return lastMin; }
    }

    public int Hour
    {
        get { return lastHour; }
    }

    public bool Paused
    {
        get { return isPaused; }
    }

    public string getTime()
    {
        return timeText.text;
    }
    #endregion
}
