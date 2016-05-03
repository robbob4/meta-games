using UnityEngine;
using System.Collections;

public class ConstructionButton : MonoBehaviour
{
    #region Reference variables
    private Constructor theConstructor = null;
    #endregion

	// Use this for initialization
	void Start ()
    {
        #region References
        theConstructor = GameObject.Find("GameManager").GetComponent<Constructor>();
        if (theConstructor == null)
        {
            Debug.LogError("theConstructor not found for " + this + ".");
            Application.Quit();
        }
        #endregion
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetAxis("Jump") >= 1) //imiate a UI click or hotkey for a room
        {
            //call construction mode with a room object parameter
            theConstructor.EnterConstructionMode(new Shop());
                //attach a room to the mouse
                //update room center with mouse position adjusted by position threshold

        }
	}
}
