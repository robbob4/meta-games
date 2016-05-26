// ------------------------- GlobalGameManager.cs -----------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 18, 2016
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

    [SerializeField] private double cash = 1000000.0; //starting funds
    private bool paused = false; //tracks whether the game is paused currently
    private Vector3 spawnPosition = new Vector3(37.5f, 3.2f, -5.0f);
    private float statusDelay = 30.0f; //time in real seconds before messages are cleared
    private float currentStatusDelay = 0.0f; //used to track current status message delay
    private float tempStatusDelay = 0.0f;
    private string oldStatusText = "Status:";
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
    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region Cash update
        //todo: deduct funds for maint at a certain time
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

        Debug.Log(query + " sound effect was not found.");
        return null;
    }
    #endregion
}
