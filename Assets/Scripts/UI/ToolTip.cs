// ------------------------------ ToolTip.cs ----------------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - May 25, 2016
// Modified - May 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a UI description box used in conjunction with 
// the ButtonController to display information about the rooms.
// ----------------------------------------------------------------------------
// Notes - Calling any setter will cause the tooltip to be visible.
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public class ToolTip : MonoBehaviour
{
    #region Variables
	//references
	private GameObject toolTipObj = null;
	private Text roomName = null;
	private Text roomProfit = null;
	private Text roomHappiness = null;
	private Text roomCapacity = null;
    private Image roomInterest = null;
	private Vector3 target = new Vector3(0, 0, 0);
	private GameObject[] tipElements = null;

	NumberFormatInfo format = null;
    private bool visible = false;
    private float halfWidth = 0;
    private float halfHeight = 0;
	#endregion

	void Awake()
	{
		tipElements = GameObject.FindGameObjectsWithTag("ToolTip");
	}

	// Use this for initialization
	void Start ()
	{
		toolTipObj = GameObject.Find("ToolTip");
		if (toolTipObj == null)
			Debug.Log("Unable to find ToolTip for " + this + ".");

		roomName = GameObject.Find("RoomName").GetComponent<Text>();
		if (roomName == null)
			Debug.Log("Unable to find RoomName for " + this + ".");

        roomInterest = GameObject.Find("RoomInterest").GetComponent<Image>();
        if (roomInterest == null)
            Debug.Log("Unable to find RoomInterest for " + this + ".");

        roomProfit = GameObject.Find("RoomProfit").GetComponentsInChildren<Text>()[1];
		if (roomProfit == null)
			Debug.Log("Unable to find RoomProfit for " + this + ".");

		roomHappiness = GameObject.Find("RoomHappiness").GetComponentsInChildren<Text>()[1];
		if (roomHappiness == null)
			Debug.Log("Unable to find RoomHappiness for " + this + ".");
		
		roomCapacity = GameObject.Find("RoomCapacity").GetComponentsInChildren<Text>()[1];
		if (roomCapacity == null)
			Debug.Log("Unable to find RoomCapacity for " + this + ".");

		format = new NumberFormatInfo();
		format.CurrencyDecimalDigits = 0; //remove decminal from the number format

        var rectTransform = GetComponent<RectTransform>();
        halfWidth = rectTransform.rect.width / 2;
        halfHeight = rectTransform.rect.height / 2;

        //hide the panel
        toolTipObj.GetComponent<Image>().enabled = false;

		for (int i = 0; i < tipElements.Length; i++)
        {
			tipElements[i].SetActive(false);
		}

	}

	// Update is called once per frame
	void Update ()
	{
		calculateTarget();
		this.transform.position = target;
	}

    #region Getters and Setters
    // Changes the title of the tooltip
    public void SetName(string title)
    {
        if (!visible)
            ShowTooltip();

        roomName.text = title;
    }

    // Changes the interest icon
    public void SetInterest(Patron.Interest newInterest)
    {
        if (!visible)
            ShowTooltip();

        Sprite temp = null;

        switch (newInterest)
        {
            case Patron.Interest.Entertainment:
                temp = Resources.Load<Sprite>("Textures/UI/Interests/Entertainment");
                if (temp == null)
                {
                    Debug.Log("Unable to find Textures/UI/Interests/Entertainment for " + this);
                    return;
                }
                break;

            case Patron.Interest.Fashion:
                temp = Resources.Load<Sprite>("Textures/UI/Interests/Fashion");
                if (temp == null)
                {
                    Debug.Log("Unable to find Textures/UI/Interests/Fashion for " + this);
                    return;
                }
                break;

            case Patron.Interest.Gadgets:
                temp = Resources.Load<Sprite>("Textures/UI/Interests/Gadgets");
                if (temp == null)
                {
                    Debug.Log("Unable to find Textures/UI/Interests/Gadgets for " + this);
                    return;
                }
                break;

            case Patron.Interest.Health:
                temp = Resources.Load<Sprite>("Textures/UI/Interests/Health");
                if (temp == null)
                {
                    Debug.Log("Unable to find Textures/UI/Interests/Health for " + this);
                    return;
                }
                break;

            case Patron.Interest.Savory:
                temp = Resources.Load<Sprite>("Textures/UI/Interests/Savory");
                if (temp == null)
                {
                    Debug.Log("Unable to find Textures/UI/Interests/Savory for " + this);
                    return;
                }
                break;

            case Patron.Interest.Spicy:
                temp = Resources.Load<Sprite>("Textures/UI/Interests/Spicy");
                if (temp == null)
                {
                    Debug.Log("Unable to find Textures/UI/Interests/Spicy for " + this);
                    return;
                }
                break;

            case Patron.Interest.Sweet:
                temp = Resources.Load<Sprite>("Textures/UI/Interests/Sweet");
                if (temp == null)
                {
                    Debug.Log("Unable to find Textures/UI/Interests/Sweet for " + this);
                    return;
                }
                break;

            default:
                temp = Resources.Load<Sprite>("Textures/UI/Interests/None");
                if (temp == null)
                {
                    Debug.Log("Unable to find Textures/UI/Interests/None for " + this);
                    return;
                }
                break;
        }

        roomInterest.sprite = temp;
    }

    // Displayed without any decimal
    public void SetProfit(int profit)
    {
        if (!visible)
            ShowTooltip();

        roomProfit.text = profit.ToString("c", format);
    }

    // Appends % to the end of given string
    public void SetHappiness(int happiness)
    {
        if (!visible)
            ShowTooltip();

        roomHappiness.text = happiness + "%";
    }

    // Changes the current and max capacity values with a " / " in between
    public void SetCapacity(int current, int max)
    {
        if (!visible)
            ShowTooltip();

        roomCapacity.text = current + " / " + max;
    }

    public void HideTooltip()
    {
        visible = false;
        GetComponent<Image>().enabled = false;

        for (int i = 0; i < tipElements.Length; i++)
        {
            tipElements[i].SetActive(false);
        }
    }

    public void ShowTooltip()
    {
        visible = true;
        GetComponent<Image>().enabled = true;

        for (int i = 0; i < tipElements.Length; i++)
        {
            tipElements[i].SetActive(true);
        }
    }
    #endregion

    void calculateTarget()
    {
        //current mouse position
        Vector3 currMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        target = currMousePosition;
        target.x -= halfWidth;
        target.y += halfHeight;
    }
}