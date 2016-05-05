using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTime : MonoBehaviour {

    [HideInInspector] public bool isPaused;
    [SerializeField] private float timeRatio = 1.0f;     //Game Minutes to RL Seconds
    private Text timeText;
    private float theTime;
    private float stoppedTime;

	void Start ()
    {
        isPaused = false;
        timeText = GameObject.Find("Information/World Time").GetComponent<Text>();
        theTime = Time.time;
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
            theTime = ((Time.time - stoppedTime) * timeRatio);
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

        if ( mins < 10)
        {
            sMins = "0" + mins;
        }
        else
        {
            sMins = "" + mins;
        }

        hours = ((int)theTime / 60) %12;

        if (hours == 0)
        {
            sHours = "12";
        }
        else
        {
            sHours = "" + hours;
        }

        timeText.text = sHours + ":" + sMins;
    }
}
