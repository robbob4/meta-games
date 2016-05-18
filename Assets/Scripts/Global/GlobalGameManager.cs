// ------------------------- GlobalGameManager.cs -----------------------------
// Author - Robert Griswold CSS 385
// Created - May 12, 2016
// Modified - May 12, 2016
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
    #region Variables
    private bool paused = false;
    private double cash = 1000000.0;
    private Destination lobby = null;
    private Text cashDisplay = null;
    private Vector3 spawnPosition = new Vector3(37.5f, 3.2f, -5.0f);
    #endregion

    // Use this for fast initialization
    void Awake()
    {
        lobby = GameObject.Find("Lobby").GetComponent<Destination>();
        if (lobby == null)
            Debug.LogError("Lobby not found for " + this + ".");

        cashDisplay = GameObject.Find("Cash").GetComponent<Text>();
        if (cashDisplay == null)
            Debug.LogError("cashDisplay not found for " + this + ".");
    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        cashDisplay.text = cash.ToString("c");
	}

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
        get { return lobby; }
    }

    public Vector3 SpawnPosition
    {
        get { return spawnPosition; }
    }
    #endregion
}
