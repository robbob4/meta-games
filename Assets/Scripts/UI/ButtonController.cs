// ------------------------ ButtonController.cs -------------------------------
// Author - Brent Eaves CSS 385
// Author - Robert Griswold CSS 385
// Created - May 4, 2016
// Modified - May 12, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a UI button controller.
// ----------------------------------------------------------------------------
// Notes - (Needs comments).
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{

    public enum ToolType
    {
        Inspect,
        Build,
        Destroy
    }
    public enum RoomType
    {
        Lobby = 0,
        Shop = 1,
        Office = 3,
        Hotel = 4
    }

    private ToolType currentTool = ToolType.Inspect;
    private RoomType buildRoom;
    private GameObject referenceRoom;
    private GameObject icon;
    private RoomStats descriptionBox = null;

	void Start ()
    {
        #region References
        icon = GameObject.Find("BuildObject") as GameObject;
        if (icon == null)
            Debug.LogError("BuildObject not found for " + this + ".");

        descriptionBox = GameObject.Find("RoomStats").GetComponent<RoomStats>();
        if (descriptionBox == null)
            Debug.LogError("RoomStats not found for " + this + ".");
        #endregion
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentTool = ToolType.Inspect;
            if (descriptionBox != null)
                descriptionBox.SelectNone();
        }

        Vector3 iconPlace;
        switch (currentTool)
        {
            case ToolType.Inspect:
                iconPlace = Camera.main.ScreenToWorldPoint(new Vector3(-.1f, -.1f));
                icon.transform.position = iconPlace;
                break;
            case ToolType.Build:
                iconPlace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                iconPlace.z = -1f;
                icon.transform.position = iconPlace;
                break;
            case ToolType.Destroy:
                iconPlace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                iconPlace.z = -1f;
                icon.transform.position = iconPlace;
                break;
            default:
                break;
        }
	}

    public void SelectInspect()
    {
        referenceRoom = null;
        currentTool = ToolType.Inspect;
    }

    public void SelectRoom(int type)
    {
        currentTool = ToolType.Build;
        Sprite s = Resources.Load<Sprite>("Textures/BrentDemo/TestRoom");
        SpriteRenderer r = icon.GetComponent<SpriteRenderer>();
        icon.transform.localScale = new Vector3(5f, 5f, 1f);
        r.sprite = s;

        switch (type)
        {
            case 0:
                referenceRoom = Resources.Load("Prefabs/BrentDemo/RoomLobby") as GameObject;
                r.sprite = s;
                r.color = new Color(.9F, .5F, .1F);
                break;
            case 1:
                referenceRoom = Resources.Load("Prefabs/BrentDemo/RoomShop") as GameObject;
                r.sprite = s;
                r.color = new Color(.35F, .8F, .95F);
                break;
            case 2:
                referenceRoom = Resources.Load("Prefabs/BrentDemo/RoomOffice") as GameObject;
                r.sprite = s;
                r.color = new Color(.45F, .9F, .7F);
                break;
            case 3:
                referenceRoom = Resources.Load("Prefabs/BrentDemo/RoomHotel") as GameObject;
                r.sprite = s;
                r.color = new Color(.7F, .2F, .85F);
                break;
            default:
                break;
        }
    }

    public void SelectDestruct()
    {
        referenceRoom = null;
        currentTool = ToolType.Destroy;
        icon.GetComponent<SpriteRenderer>().sprite = null;
    }
}