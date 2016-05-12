// ----------------------------- MenuBehavior.cs ------------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - Apr 25, 2016
// Modified - May 12, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for main menu behavior where the buttons are used 
// to change scenes.
// ----------------------------------------------------------------------------
// Notes - Credits button simply hides some UI elements
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement; //for SceneManager
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    #region Variables
    [HideInInspector] public Button NewGameButton;
    [HideInInspector] public Button LoadGameButton;
    [HideInInspector] public Button CreditsButton;
    [HideInInspector] public Button QuitButton;

    private int quitDelay = 80; //number of frames to hold esc/back to quit
    private int quitDelayCount = 80;
    private GameObject[] menuElements;
    #endregion

    // Use this for initialization
    void Start ()
    {
        #region Button init
        //find buttons
        NewGameButton = GameObject.Find("ButtonNew").GetComponent<Button>();
        if (NewGameButton == null)
            Debug.LogError("ButtonNew not found.");
        LoadGameButton = GameObject.Find("ButtonLoad").GetComponent<Button>();
        if (LoadGameButton == null)
            Debug.LogError("ButtonLoad not found.");
        CreditsButton = GameObject.Find("ButtonCredits").GetComponent<Button>();
        if (CreditsButton == null)
            Debug.LogError("ButtonCredits not found.");
        QuitButton = GameObject.Find("ButtonQuit").GetComponent<Button>();
        if (QuitButton == null)
            Debug.LogError("ButtonQuit not found.");

        //add listeners to the buttons
        NewGameButton.onClick.AddListener(NewGameButtonService);
        LoadGameButton.onClick.AddListener(LoadGameButtonService);
        CreditsButton.onClick.AddListener(CreditsButtonService);
        QuitButton.onClick.AddListener(QuitButtonService);
        #endregion

        #region Hide credits
        menuElements = GameObject.FindGameObjectsWithTag("UI");

        for (int i = 0; i < menuElements.Length; i++)
        {
            if (menuElements[i].name == "Credits")
                menuElements[i].SetActive(false);
        }
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

            HideCredits();
        }
        else
        {
            //reset quit delay
            quitDelayCount = quitDelay;
        }
    }

    #region Button service functions
    private void NewGameButtonService()
    {
		LoadScene("BrentDemo");
	}

    private void LoadGameButtonService()
    {
        LoadScene("BrentDemo");
    }

    private void CreditsButtonService()
    {
        //hide buttons and props but show credits
        for (int i = 0; i < menuElements.Length; i++)
        {
            if (menuElements[i].name == "Credits")
                menuElements[i].SetActive(true);
            else
                menuElements[i].SetActive(false);
        }
    }

    private void QuitButtonService()
    {
        Application.Quit(); //only works in build - not debug
    }

    public void HideCredits()
    {
        //show buttons and props but no credits
        for (int i = 0; i < menuElements.Length; i++)
        {
            if (menuElements[i].name == "Credits")
                menuElements[i].SetActive(false);
            else
                menuElements[i].SetActive(true);
        }
    }
    #endregion

    #region Change scene support
    void LoadScene(string theLevel)
    {
		SceneManager.LoadScene(theLevel);
	}
    #endregion
}