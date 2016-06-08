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

    private Gradient dayGradient;
    private Gradient darknessGradient;
    private Camera mainCamera = null;
    private Renderer bgMat = null;

    private float theTime;
    private float stoppedTime;
    private float timeRatio = 40.0f; //Game Minutes to RL Seconds
    private float offset = 620; //7am prestart
    private int lastHour;
    private int lastMin;
    private bool pm;

    private float kidVolume = 0.0f;
    private AudioSource kidPlayer = null;
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
        kidPlayer = GameObject.Find("Ambience").GetComponent<AudioSource>();
        if (kidPlayer == null)
            Debug.LogError("Ambience not found for " + this + ".");
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (mainCamera == null)
            Debug.LogError("Camera not found for " + this + ".");
        bgMat = GameObject.Find("Background").GetComponent<Renderer>();
        if (bgMat == null)
            Debug.LogError("Background not found for " + this + ".");

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

        #region BG Gradient
        GradientColorKey[] dayColor;
        GradientAlphaKey[] dayAlpha;
        dayGradient = new Gradient();
        dayColor = new GradientColorKey[8];
        dayColor[0].color = new Color32(8, 8, 8, 255);
        dayColor[0].time = 0.0f; //darkness 0000
        dayColor[1].color = new Color32(8, 8, 8, 255);
        dayColor[1].time = 0.2083f; //night 0500
        dayColor[2].color = new Color32(241, 152, 106, 255);
        dayColor[2].time = 0.25f; //dawn 0600
        dayColor[3].color = new Color32(121, 148, 167, 255);
        dayColor[3].time = 0.2916f; //day 0700
        dayColor[4].color = new Color32(67, 102, 137, 255);
        dayColor[4].time = 0.7916f; //day 1900
        dayColor[5].color = new Color32(198, 115, 101, 255);
        dayColor[5].time = 0.833f; //dusk 2000
        dayColor[6].color = new Color32(34, 35, 42, 255);
        dayColor[6].time = 0.875f; //night 2100
        dayColor[7].color = new Color32(8, 8, 8, 255);
        dayColor[7].time = 1.0f; //darkness 2400
        dayAlpha = new GradientAlphaKey[2];
        dayAlpha[0].alpha = 1.0F;
        dayAlpha[0].time = 0.0F;
        dayAlpha[1].alpha = 1.0F;
        dayAlpha[1].time = 1.0F;
        dayGradient.SetKeys(dayColor, dayAlpha);

        GradientColorKey[] darknessColor;
        GradientAlphaKey[] darknessAlpha;
        darknessGradient = new Gradient();
        darknessColor = new GradientColorKey[8];
        darknessColor[0].color = new Color32(40, 40, 40, 255);
        darknessColor[0].time = 0.0f; //darkness 0000
        darknessColor[1].color = new Color32(40, 40, 40, 255);
        darknessColor[1].time = 0.2083f; //night 0500
        darknessColor[2].color = new Color32(150, 150, 150, 255);
        darknessColor[2].time = 0.25f; //dawn 0600
        darknessColor[3].color = new Color32(255, 255, 255, 255);
        darknessColor[3].time = 0.2916f; //day 0700
        darknessColor[4].color = new Color32(255, 255, 255, 255);
        darknessColor[4].time = 0.7916f; //day 1900
        darknessColor[5].color = new Color32(150, 150, 150, 255);
        darknessColor[5].time = 0.833f; //dusk 2000
        darknessColor[6].color = new Color32(40, 40, 40, 255);
        darknessColor[6].time = 0.875f; //night 2100
        darknessColor[7].color = new Color32(40, 40, 40, 255);
        darknessColor[7].time = 1.0f; //darkness 2400
        darknessAlpha = new GradientAlphaKey[2];
        darknessAlpha[0].alpha = 1.0F;
        darknessAlpha[0].time = 0.0F;
        darknessAlpha[1].alpha = 1.0F;
        darknessAlpha[1].time = 1.0F;
        darknessGradient.SetKeys(darknessColor, darknessAlpha);
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

        #region Ambience
        //fade in
        if (kidVolume < 0.5f)
        {
            kidVolume += 0.1f * Time.deltaTime;
        }

        //set volume
        if (kidVolume != 0.0f || kidVolume != 0.5f)
        {
            Mathf.Clamp(kidVolume, 0.0f, 0.5f);
            kidPlayer.volume = kidVolume;
        }
        #endregion

        #region TimeUpdate
        theTime = ((Time.time - stoppedTime) * timeRatio) + offset;
        convertTime();
        #endregion

        #region BG Color
        //calculate time
        float hour = Hour24 / 24f;
        float min = Min * 0.000694f;

        //set the color
        mainCamera.backgroundColor = dayGradient.Evaluate(hour + min);
        bgMat.material.SetColor("_Color", darknessGradient.Evaluate(hour + min));
        #endregion
    }

    #region Button service functions
    private void NewGameButtonService()
    {
		LoadScene("Game");
	}

    private void LoadGameButtonService()
    {
        LoadScene("Game");
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

    #region Time
    void convertTime()
    {
        int hours, mins;
        string sHours, sMins;

        mins = (int)theTime % 60;

        //adjust single digit mins
        if (mins < 10)
        {
            sMins = "0" + mins;
        }
        else
        {
            sMins = "" + mins;
        }

        hours = ((int)theTime / 60) % 12;

        //toggle am/pm
        if (lastHour == 11 && hours == 0)
            pm = !pm;

        lastHour = hours;
        lastMin = mins;
    }

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

    public int Hour24
    {
        get
        {
            if (PM)
                return lastHour + 12;
            else
                return lastHour;
        }
    }
    #endregion
}