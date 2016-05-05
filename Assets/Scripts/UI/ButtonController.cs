// ------------------------ ButtonController.cs -------------------------------
// Author - Brent Eaves CSS 385
// Created - May 4, 2016
// Modified - May 4, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a UI button controller for the protoype demo.
// ----------------------------------------------------------------------------
// Notes - (Needs comments).
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

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
    private ToolType currentTool;
    private RoomType buildRoom;
    private GameObject refrenceRoom;
    private GameObject icon;

	void Start ()
    {
	    icon = GameObject.Find("BuildObject") as GameObject;
        currentTool = ToolType.Inspect;
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentTool = ToolType.Inspect;
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
        refrenceRoom = null;
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
                refrenceRoom = Resources.Load("Prefabs/BrentDemo/RoomLobby") as GameObject;
                r.sprite = s;
                r.color = new Color(.9F, .5F, .1F);
                break;
            case 1:
                refrenceRoom = Resources.Load("Prefabs/BrentDemo/RoomShop") as GameObject;
                r.sprite = s;
                r.color = new Color(.35F, .8F, .95F);
                break;
            case 2:
                refrenceRoom = Resources.Load("Prefabs/BrentDemo/RoomOffice") as GameObject;
                r.sprite = s;
                r.color = new Color(.45F, .9F, .7F);
                break;
            case 3:
                refrenceRoom = Resources.Load("Prefabs/BrentDemo/RoomHotel") as GameObject;
                r.sprite = s;
                r.color = new Color(.7F, .2F, .85F);
                break;
            default:
                break;
        }
    }

    public void SelectDestruct()
    {
        refrenceRoom = null;
        currentTool = ToolType.Destroy;
        icon.GetComponent<SpriteRenderer>().sprite = null;
    }
}