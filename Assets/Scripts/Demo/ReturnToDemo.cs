// -------------------------- ReturnToDemo.cs ---------------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - Apr 25, 2016
// Modified - April 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation to return to the DemoHub by hitting the ESC/Back 
// button.
// ----------------------------------------------------------------------------
// Notes - Script for demo scenes.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToDemo : MonoBehaviour
{	
	//Update is called once per frame
	void Update ()
    {
		//if (Input.GetKey (KeyCode.Escape))
        if (Input.GetAxis("Cancel") == 1)
        {
			SceneManager.LoadScene("DemoHub");
		}
	}
}
