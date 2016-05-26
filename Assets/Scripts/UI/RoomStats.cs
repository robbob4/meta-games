// ---------------------------- RoomStats.cs ----------------------------------
// Author - Brent Eaves CSS 385
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - May 4, 2016
// Modified - May 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a UI description box used in conjunction with 
// the ButtonController to display information about the rooms.
// ----------------------------------------------------------------------------
// Notes - (Needs more comments).
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public class RoomStats : MonoBehaviour {

    #region Variables
    //references
    private GameObject roomStatsPanel = null;
    private Text titleText = null;
    private Text costText = null;
    private Text upkeepText = null;
    private Text sizeText = null;
    private Text descText = null;

    NumberFormatInfo format = null;
    #endregion

    // Use this for initialization
    void Start ()
    {
        roomStatsPanel = GameObject.Find("RoomStats");
        if (roomStatsPanel == null)
            Debug.Log("Unable to find RoomStats for " + this + ".");

        titleText = GameObject.Find("RoomType").GetComponent<Text>();
        if (titleText == null)
            Debug.Log("Unable to find RoomType for " + this + ".");

        costText = GameObject.Find("RoomCost").GetComponentsInChildren<Text>()[1];
        if (costText == null)
            Debug.Log("Unable to find RoomCost for " + this + ".");

        upkeepText = GameObject.Find("RoomUpkeep").GetComponentsInChildren<Text>()[1];
        if (upkeepText == null)
            Debug.Log("Unable to find RoomUpkeep for " + this + ".");

        sizeText = GameObject.Find("RoomSize").GetComponentsInChildren<Text>()[1];
        if (sizeText == null)
            Debug.Log("Unable to find RoomSize for " + this + ".");

        descText = GameObject.Find("RoomDescription").GetComponentsInChildren<Text>()[1];
        if (descText == null)
            Debug.Log("Unable to find RoomDescription for " + this + ".");

        format = new NumberFormatInfo();
        format.CurrencyDecimalDigits = 0; //remove decminal from the number format

        roomStatsPanel.SetActive(false); //hide the panel
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public string getTitle()
    {
        return titleText.text;
    }

    public void SetTitle(string title)
    {
        titleText.text = title;
        roomStatsPanel.SetActive(true);
    }

    public void SetCost(int cost)
    {
        
        costText.text = cost.ToString("c", format);
    }

    public void SetUpkeep(int upkeep)
    {
        upkeepText.text = upkeep.ToString("c", format);
    }

    public void SetSize(Destination.Size size)
    {
        switch (size)
        {
            case Destination.Size.Tiny:
                sizeText.text = "Tiny";
                break;

            case Destination.Size.Small:
                sizeText.text = "Small";
                break;

            case Destination.Size.Medium:
                sizeText.text = "Medium";
                break;

            case Destination.Size.Large:
                sizeText.text = "Large";
                break;
            default:
                sizeText.text = "";
                break;
        }  
    }

    public void SetDesc(string desc)
    {
        descText.text = desc;
    }

    public void SelectNone()
    {
        //titleText.GetComponent<Text>().text = "";
        //costText.text = "";
        //upkeepText.text = "";
        //sizeText.text = "";
        //descText.text = "";
        roomStatsPanel.SetActive(false);
    }
}