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
using UnityEngine.SceneManagement;
using System.Collections;
using System.Globalization;

public class GlobalGameManager : MonoBehaviour
{
    #region Structs
    private struct SoundEffect
    {
        public string name;
        public AudioSource audioSource;
    }
    #endregion

    #region Enums
    public enum AmbiencePlaying { low, high, kid}
    #endregion

    #region Variables
    //references
    private Destination fakeLobby = null;
    private Text cashDisplay = null;
    private Text statusDisplay = null;
    private GameTime gameTime = null;
    private Text trafficButton = null;
    private SoundEffect[] soundEffects;
    private Camera mainCamera = null;
    private Renderer bgMat = null;
    private NumberFormatInfo format;

    private double cash = 1000000.0; //starting funds
    private int loanDelay = 0; //counter until loan must be repaid. -1 means no loan
    private float loanAmount = 0.0f;
    private bool loanWait = false;
    private bool paused = false; //tracks whether the game is paused currently
    private Vector3 spawnPosition;

    float lowVolume = 0.0f;
    float highVolume = 0.0f;
    float kidVolume = 0.0f;
    private AmbiencePlaying currentAmbience = AmbiencePlaying.low;

    private float statusDelay = 30.0f; //time in real seconds before messages are cleared
    private float currentStatusDelay = 0.0f; //used to track current status message delay
    private float tempStatusDelay = 0.0f;
    private string oldStatusText = "Status:";
    
    private Gradient dayGradient;
    private Gradient darknessGradient;
    #endregion

    // Use this for fast initialization
    void Awake()
    {
        #region References
        fakeLobby = GameObject.Find("FakeLobby").GetComponent<Destination>();
        if (fakeLobby == null)
            Debug.LogError("FakeLobby destination not found for " + this + ".");
        spawnPosition = new Vector3(fakeLobby.transform.position.x + Constructor.UNIT_WIDTH, fakeLobby.transform.position.y - 2, -5.0f);

        cashDisplay = GameObject.Find("Cash").GetComponent<Text>();
        if (cashDisplay == null)
            Debug.LogError("cashDisplay not found for " + this + ".");

        statusDisplay = GameObject.Find("Status").GetComponentInChildren<Text>();
        if (statusDisplay == null)
            Debug.LogError("statusDisplay not found for " + this + ".");

        trafficButton = GameObject.Find("Traffic").GetComponentInChildren<Text>();
        if (trafficButton == null)
            Debug.LogError("Traffic not found for " + this + ".");

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

        format = new NumberFormatInfo();
        format.CurrencyDecimalDigits = 0; //remove decminal from the number format
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
	
	// Update is called once per frame
	void Update ()
    {
        #region Loans
        if (gameTime.Hour24 == 5 && loanAmount > 0 && !loanWait) //5am, existing loan, not displayed yet
        {
            loanWait = true;

            if (Money >= loanAmount && loanDelay == 0) //Pay the loan if last quarter and enough money
            {
                Deduct(loanAmount);
                NewStatus("Loan of " + loanAmount.ToString("c", format) + " paid off!", true);
                GetSoundEffect("maint_s").Play();
                loanAmount = 0;
                loanWait = false;
            }
            else if (loanDelay == 0) //lose the game if last quarter and not enough money
            {
                NewStatus("Unable to pay off loan of " + loanAmount.ToString("c", format) + ".", false);
                GetSoundEffect("floor_s").Play();
                Paused = true;
                StartCoroutine(ExitDelay(2));
            }
            else //remind user of loan
            {
                NewStatus("Loan of " + loanAmount.ToString("c", format) + " due in " + loanDelay + " quarter(s).", true);
            }
        }
        else if(gameTime.Hour24 == 5 && Money < 0 && loanAmount == 0 && !loanWait) //5am, no loan, not displayed yet
        {
            loanWait = true;
            loanAmount = (float)Money * -1.0f + 500000f;
            Payment(loanAmount);
            GetSoundEffect("cash_s").Play();
            loanDelay = 4; //number of quarters to repay
            NewStatus("Loan of " + loanAmount.ToString("c", format) + " due in " + loanDelay + " quarter(s).", true);
        }
        else if(gameTime.Hour24 != 5 && loanAmount != 0 && loanWait) //not 5am, reset bool and decrement counter
        {
            loanWait = false;
            loanDelay--;
        }
        #endregion

        #region UI updates
        //display cash
        cashDisplay.text = cash.ToString("c", format);
        if (cash < 0)
            cashDisplay.color = Color.red;
        else
            cashDisplay.color = new Color32(50, 50, 50, 255);

        //determine traffic amount for rooms
        int avgTraffic = 0;
        int count = 0;
        Retail[] retail = GameObject.FindObjectsOfType<Retail>();
        Leased[] leased = GameObject.FindObjectsOfType<Leased>();
        for (int i = 0; i < retail.Length; i++)
        {
            if (!retail[i].Temp)
            {
                count++;
                avgTraffic += retail[i].Traffic;
            }
        }
            
        for (int i = 0; i < leased.Length; i++)
        {
            if (!leased[i].Temp)
            {
                count++;
                avgTraffic += leased[i].Traffic;
            }
        }
            
                
        if(count != 0)
            avgTraffic /= count;
        trafficButton.text = avgTraffic + "%";
        #endregion

        #region Ambience
        //select current
        Patron[] patrons = GameObject.FindObjectsOfType<Patron>();
        if (patrons.Length >= 30)
        {
            currentAmbience = AmbiencePlaying.kid;
        }
        else if (patrons.Length >= 15)
        {
            currentAmbience = AmbiencePlaying.high;
        }
        else
        {
            currentAmbience = AmbiencePlaying.low;
        }

        //fade out
        switch (currentAmbience)
        {
            case AmbiencePlaying.low:
                if (highVolume > 0.0f)
                {
                    highVolume -= 0.15f * Time.deltaTime;
                }
                if (kidVolume > 0.0f)
                {
                    kidVolume -= 0.15f * Time.deltaTime;
                }
                break;
            case AmbiencePlaying.high:
                if (lowVolume > 0.0f)
                {
                    lowVolume -= 0.15f * Time.deltaTime;
                }
                if (kidVolume > 0.0f)
                {
                    kidVolume -= 0.15f * Time.deltaTime;
                }
                break;
            case AmbiencePlaying.kid:
                if (highVolume > 0.0f)
                {
                    highVolume -= 0.15f * Time.deltaTime;
                }
                if (lowVolume > 0.0f)
                {
                    lowVolume -= 0.15f * Time.deltaTime;
                }
                break;
        }

        //fade in
        switch (currentAmbience)
        {
            case AmbiencePlaying.low:
                if (lowVolume < 1.0f)
                {
                    lowVolume += 0.1f * Time.deltaTime;
                }
                break;
            case AmbiencePlaying.high:
                if (highVolume < 1.0f)
                {
                    highVolume += 0.1f * Time.deltaTime;
                }
                break;
            case AmbiencePlaying.kid:
                if (kidVolume < 1.0f)
                {
                    kidVolume += 0.1f * Time.deltaTime;
                }
                break;
        }

        //set volume
        if(lowVolume != 0.0f || lowVolume != 1.0f)
        {
            Mathf.Clamp(lowVolume, 0.0f, 1.0f);
            GetSoundEffect("low_ambience_s").volume = lowVolume;
        }
        if (highVolume != 0.0f || highVolume != 1.0f)
        {
            Mathf.Clamp(highVolume, 0.0f, 1.0f);
            GetSoundEffect("high_ambience_s").volume = highVolume;
        }
        if (kidVolume != 0.0f || kidVolume != 1.0f)
        {
            Mathf.Clamp(kidVolume, 0.0f, 1.0f);
            GetSoundEffect("kid_ambience_s").volume = kidVolume;
        }
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
            statusDisplay.color = new Color32(50, 50, 50, 255);
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

    //exit to main menu
    private IEnumerator ExitDelay(int delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("MainMenu");
    }

    #region Audio functions
    private void fadeOut()
    {
        
    }
    #endregion

    #region Status functions
    //sets status message with game time if logging is specified
    //non logged messages are not repeated, last a quarter of the time, and restore previous message
    public void NewStatus(string message, bool logging)
    {
        if (logging)
        {
            statusDisplay.text = "Status: [" + gameTime.getTime() + "] " + message; //todo: add new message to a log that can be accessed later
            statusDisplay.color = new Color32(50, 50, 50, 255);
            GetSoundEffect("notification_s").Play();
            currentStatusDelay = statusDelay;
            
        }      
        else if (statusDisplay.text != "Status: " + message)
        {
            oldStatusText = statusDisplay.text;
            statusDisplay.text = "Status: " + message;
            statusDisplay.color = Color.red;
            GetSoundEffect("notification_s").Play();
            tempStatusDelay = statusDelay / 20;
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
