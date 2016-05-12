// -------------------------- ReturnMain.cs ---------------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - Apr 25, 2016
// Modified - May 15, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation to return to the Main Menu by hitting the ESC/Back 
// button.
// ----------------------------------------------------------------------------
// Notes - None.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMain : MonoBehaviour
{
    //Update is called once per frame
    void Update()
    {
        //if (Input.GetKey (KeyCode.Escape))
        if (Input.GetAxis("Cancel") == 1)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
