using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomStats : MonoBehaviour {

    private GameObject titleText = null;

	// Use this for initialization
	void Start ()
    {
        titleText = GameObject.Find("RoomType");
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void SelectLobby ()
    {
        titleText.GetComponent<Text>().text = "Lobby";
    }

    public void SelectShop()
    {
        titleText.GetComponent<Text>().text = "Shop";
    }

    public void SelectOffice()
    {
        titleText.GetComponent<Text>().text = "Office";
    }

    public void SelectHotel()
    {
        titleText.GetComponent<Text>().text = "Hotel";
    }
}
