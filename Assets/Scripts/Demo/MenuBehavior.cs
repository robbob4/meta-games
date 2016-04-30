// ----------------------------- MenuBehavior.cs ------------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - Apr 25, 2016
// Modified - April 30, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for main menu behavior where the buttons are used 
// to change scenes.
// ----------------------------------------------------------------------------
// Notes - Script for demo scenes.
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement; //for SceneManager
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour
{
    #region Variables
    [HideInInspector] public Button RobertDemoButton;
    [HideInInspector] public Button SamDemoButton;
    [HideInInspector] public Button BrentDemoButton;
    [HideInInspector] public Button QuitButton;

    private int quitDelay = 80; //number of frames to hold esc/back to quit
    private int quitDelayCount = 80;
    #endregion

    // Use this for initialization
    void Start ()
    {
        #region Button init
        //find buttons
        RobertDemoButton = GameObject.Find("ButtonRobert").GetComponent<Button>();
        if (RobertDemoButton == null)
            Debug.LogError("ButtonRobert not found.");
        SamDemoButton = GameObject.Find("ButtonSam").GetComponent<Button>();
        if (SamDemoButton == null)
            Debug.LogError("ButtonSam not found.");
        BrentDemoButton = GameObject.Find("ButtonBrent").GetComponent<Button>();
        if (BrentDemoButton == null)
            Debug.LogError("ButtonBrent not found.");
        QuitButton = GameObject.Find("ButtonQuit").GetComponent<Button>();
        if (QuitButton == null)
            Debug.LogError("ButtonQuit not found.");

        //add listeners to the buttons
        RobertDemoButton.onClick.AddListener(RobertButtonService);
        SamDemoButton.onClick.AddListener(SamButtonService);
        BrentDemoButton.onClick.AddListener(BrentButtonService);
        QuitButton.onClick.AddListener(QuitButtonService);
        #endregion
    }

    //Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Escape))
        if (Input.GetAxis("Cancel") == 1)
        {
            if (quitDelayCount-- <= 0)
                QuitButtonService();
        }
        else
        {
            //reset quit delay
            quitDelayCount = quitDelay;
        }
    }

    #region Button service functions
    private void RobertButtonService()
    {
		LoadScene("RobertDemo");
	}

    private void SamButtonService()
    {
        LoadScene("SamDemo");
    }

    private void BrentButtonService()
    {
        LoadScene("BrentDemo");
    }

    private void QuitButtonService()
    {
        Application.Quit(); //only works in build - not debug
    }
    #endregion

    #region Change scene support
    void LoadScene(string theLevel)
    {
		SceneManager.LoadScene(theLevel);
	}
    #endregion
}