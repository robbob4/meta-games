// ------------------------- GlobalGameManager.cs -----------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - Jun 7, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a game manager that keep track of time, 
// money, etc.
// ----------------------------------------------------------------------------
// Notes - None.
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlobalGameManager : MonoBehaviour
{
    #region Structs
    private struct SoundEffect
    {
        public string name;
        public AudioSource audioSource;
    }
    #endregion

    #region Variables
    //references
    private Destination fakeLobby = null;
    private Text cashDisplay = null;
    private Text statusDisplay = null;
    private GameTime gameTime = null;
    private SoundEffect[] soundEffects;
    private Camera mainCamera = null;
    private Renderer bgMat = null;

    [SerializeField] private double cash = 1000000.0; //starting funds
    private bool paused = false; //tracks whether the game is paused currently
    private Vector3 spawnPosition = new Vector3(37.5f, 3.2f, -5.0f);
    private float statusDelay = 30.0f; //time in real seconds before messages are cleared
    private float currentStatusDelay = 0.0f; //used to track current status message delay
    private float tempStatusDelay = 0.0f;
    private string oldStatusText = "Status:";
    Gradient dayGradient;
    Gradient darknessGradient;
    #endregion

    // Use this for fast initialization
    void Awake()
    {
        #region References
        fakeLobby = GameObject.Find("FakeLobby").GetComponent<Destination>();
        if (fakeLobby == null)
            Debug.LogError("FakeLobby destination not found for " + this + ".");

        cashDisplay = GameObject.Find("Cash").GetComponent<Text>();
        if (cashDisplay == null)
            Debug.LogError("cashDisplay not found for " + this + ".");

        statusDisplay = GameObject.Find("Status").GetComponentInChildren<Text>();
        if (statusDisplay == null)
            Debug.LogError("statusDisplay not found for " + this + ".");

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (mainCamera == null)
            Debug.LogError("Camera not found for " + this + ".");

        bgMat = GameObject.Find("Background").GetComponent<Renderer>();
        if (bgMat == null)
            Debug.LogError("Background not found for " + this + ".");

        gameTime = this.GetComponent<GameTime>();
        if (gameTime == null)
            Debug.LogError("gameTime not found for " + this + ".");

        GameObject[] temp = GameObject.FindGameObjectsWithTag("SoundEffect");
        soundEffects = new SoundEffect[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            soundEffects[i].audioSource = temp[i].GetComponent<AudioSource>();
            soundEffects[i].name = temp[i].name;
        }
        #endregion

        #region BG Gradient
        GradientColorKey[] dayColor;
        GradientAlphaKey[] dayAlpha;
        dayGradient = new Gradient();
        dayColor = new GradientColorKey[8];
        dayColor[0].color = new Color32(8, 8, 8, 1);
        dayColor[0].time = 0.0f; //darkness 0000
        dayColor[1].color = new Color32(8, 8, 8, 1);
        dayColor[1].time = 0.2083f; //night 0500
        dayColor[2].color = new Color32(241, 152, 106, 1);
        dayColor[2].time = 0.25f; //dawn 0600
        dayColor[3].color = new Color32(121, 148, 167, 1);
        dayColor[3].time = 0.2916f; //day 0700
        dayColor[4].color = new Color32(67, 102, 137, 1);
        dayColor[4].time = 0.7916f; //day 1900
        dayColor[5].color = new Color32(198, 115, 101, 1);
        dayColor[5].time = 0.833f; //dusk 2000
        dayColor[6].color = new Color32(34, 35, 42, 1);
        dayColor[6].time = 0.875f; //night 2100
        dayColor[7].color = new Color32(8, 8, 8, 1);
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
        darknessColor[0].color = new Color32(40, 40, 40, 1);
        darknessColor[0].time = 0.0f; //darkness 0000
        darknessColor[1].color = new Color32(40, 40, 40, 1);
        darknessColor[1].time = 0.2083f; //night 0500
        darknessColor[2].color = new Color32(150, 150, 150, 1);
        darknessColor[2].time = 0.25f; //dawn 0600
        darknessColor[3].color = new Color32(255, 255, 255, 1);
        darknessColor[3].time = 0.2916f; //day 0700
        darknessColor[4].color = new Color32(255, 255, 255, 1);
        darknessColor[4].time = 0.7916f; //day 1900
        darknessColor[5].color = new Color32(150, 150, 150, 1);
        darknessColor[5].time = 0.833f; //dusk 2000
        darknessColor[6].color = new Color32(40, 40, 40, 1);
        darknessColor[6].time = 0.875f; //night 2100
        darknessColor[7].color = new Color32(40, 40, 40, 1);
        darknessColor[7].time = 1.0f; //darkness 2400
        darknessAlpha = new GradientAlphaKey[2];
        darknessAlpha[0].alpha = 1.0F;
        darknessAlpha[0].time = 0.0F;
        darknessAlpha[1].alpha = 1.0F;
        darknessAlpha[1].time = 1.0F;
        darknessGradient.SetKeys(darknessColor, darknessAlpha);
        #endregion
    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region UI updates
        cashDisplay.text = cash.ToString("c");
        #endregion

        #region Status message update
        if (currentStatusDelay > 0)
        {
            currentStatusDelay -= Time.deltaTime;
        }
        else if (currentStatusDelay < 0)
        {
            if (tempStatusDelay != 0.0f)
            {
                oldStatusText = "Status:";
            }
            else
            {
                statusDisplay.text = "Status:";
            }
            
            currentStatusDelay = 0.0f;
        }

        if (tempStatusDelay > 0)
        {
            tempStatusDelay -= Time.deltaTime;
        }
        else if (tempStatusDelay < 0)
        {
            tempStatusDelay = 0.0f;
            statusDisplay.text = oldStatusText;
            oldStatusText = "Status:";
        }
        #endregion

        #region BG Color
        //calculate time
        float hour = gameTime.Hour24 / 24f;
        float min = gameTime.Min * 0.000694f;

        //set the color
        mainCamera.backgroundColor = dayGradient.Evaluate(hour + min);
        bgMat.material.SetColor("_Color", darknessGradient.Evaluate(hour + min));
        #endregion
    }

    #region Status functions
    //sets status message with game time if logging is specified
    //non logged messages are not repeated, last a quarter of the time, and restore previous message
    public void NewStatus(string message, bool logging)
    {
        if (logging)
        {
            statusDisplay.text = "Status: [" + gameTime.getTime() + "] " + message; //todo: add new message to a log that can be accessed later
            GetSoundEffect("notification_s").Play();
            currentStatusDelay = statusDelay;
        }      
        else if (statusDisplay.text != "Status: " + message)
        {
            oldStatusText = statusDisplay.text;
            statusDisplay.text = "Status: " + message;
            GetSoundEffect("notification_s").Play();
            tempStatusDelay = statusDelay / 4;
        }        
    }
    #endregion


    #region Getters/Setters
    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }

    public double Money
    {
        get { return cash; }
    }

    public void Deduct(float amount)
    {
        if (amount > 0)
            cash -= amount;
    }

    public void Payment(float amount)
    {
        if (amount > 0)
            cash += amount;
    }

    public Destination Lobby
    {
        get { return fakeLobby; }
    }

    public Vector3 SpawnPosition
    {
        get { return spawnPosition; }
    }

    public AudioSource GetSoundEffect(string query)
    {
        for(int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].name == query)
                return soundEffects[i].audioSource;
        }

        Debug.LogError(query + " sound effect was not found.");
        return null;
    }
    #endregion
}
