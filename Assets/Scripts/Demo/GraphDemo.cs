// ---------------------------- GraphDemo.cs -------------------------------
// Author - Robert Griswold CSS 385
// Created - May 4, 2016
// Modified - May 12, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a driver for the Graph and Node class for the 
// prototype demo.
// ----------------------------------------------------------------------------
// Notes - None
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GraphDemo : MonoBehaviour
{
    //demo variables
    Graph<string> rooms;
    [HideInInspector] public Button Button1;
    [HideInInspector] public Button Button2;
    [HideInInspector] public Button Button3;
    [HideInInspector] public Button Button4;
    [HideInInspector] public Text Output;
    [HideInInspector] public Text Startup;


    // Use this for initialization
    void Start ()
    {
        //find buttons
        Button1 = GameObject.Find("Button1").GetComponent<Button>();
        if (Button1 == null)
            Debug.LogError("Button1 not found.");
        Button2 = GameObject.Find("Button2").GetComponent<Button>();
        if (Button2 == null)
            Debug.LogError("Button2 not found.");
        Button3 = GameObject.Find("Button3").GetComponent<Button>();
        if (Button3 == null)
            Debug.LogError("Button3 not found.");
        Button4 = GameObject.Find("Button4").GetComponent<Button>();
        if (Button4 == null)
            Debug.LogError("Button4 not found.");
        Output = GameObject.Find("Output").GetComponent<Text>();
        if (Output == null)
            Debug.LogError("Output not found.");
        Startup = GameObject.Find("Startup").GetComponent<Text>();
        if (Startup == null)
            Debug.LogError("Startup not found.");

        //add listeners to the buttons
        Button1.onClick.AddListener(Button1Service);
        Button2.onClick.AddListener(Button2Service);
        Button3.onClick.AddListener(Button3Service);
        Button4.onClick.AddListener(Button4Service);

        rooms = new Graph<string>();

        Startup.text = "Creating graph...\n\n";
        rooms.AddNode("Boutique");
        Startup.text += "Added Boutique\n";
        rooms.AddNode("Radio House");
        Startup.text += "Added Radio House\n";
        rooms.AddNode("Cinnaroll");
        Startup.text += "Added Cinnaroll\n";
        rooms.AddNode("Hot Dogs");
        Startup.text += "Added Hot Dogs\n";
        rooms.AddNode("Shoes");
        Startup.text += "Added Shoes\n";
        rooms.AddNode("Video Game Store");
        Startup.text += "Added Video Game Store\n\n";

        rooms.AddDirectedEdge("Radio House", "Boutique");  // Radio House -> Boutique
        Startup.text += "Linked Radio House -> Boutique\n";

        rooms.AddDirectedEdge("Boutique", "Hot Dogs");    // Boutique -> Hot Dogs
        Startup.text += "Linked Boutique -> Hot Dogs\n";
        rooms.AddDirectedEdge("Boutique", "Cinnaroll");    // Boutique -> Cinnaroll
        Startup.text += "Linked Boutique -> Cinnaroll\n";


        rooms.AddDirectedEdge("Cinnaroll", "Boutique");    // Cinnaroll -> Boutique
        Startup.text += "Linked Cinnaroll -> Boutique\n";
        rooms.AddDirectedEdge("Cinnaroll", "Radio House");    // Cinnaroll -> Radio House
        Startup.text += "Linked Cinnaroll -> Radio House\n";
        rooms.AddDirectedEdge("Cinnaroll", "Video Game Store");   // Cinnaroll -> Video Game Store
        Startup.text += "Linked Cinnaroll -> Video Game Store\n";

        rooms.AddDirectedEdge("Hot Dogs", "Cinnaroll");      // Hot Dogs -> Cinnaroll
        Startup.text += "Linked Hot Dogs -> Cinnaroll\n";
        rooms.AddDirectedEdge("Hot Dogs", "Video Game Store");   // Hot Dogs -> Video Game Store
        Startup.text += "Linked Hot Dogs -> Video Game Store\n";
        rooms.AddDirectedEdge("Hot Dogs", "Shoes");  // Hot Dogs -> Shoes
        Startup.text += "Linked Hot Dogs -> Shoes\n";

        rooms.AddDirectedEdge("Shoes", "Hot Dogs"); // Shoes -> Hot Dogs
        Startup.text += "Linked Shoes -> Hot Dogs\n";
        rooms.AddDirectedEdge("Shoes", "Radio House"); // Shoes -> Radio House
        Startup.text += "Linked Shoes -> Radio House\n";

        //Stack<string> result = web.DepthFirstSearch("Shoes");

        //Debug.Log("Reverse result:");
        //while (result.Count >= 1)
        //    Debug.Log(result.Pop());

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //function for the demo
    private void TestRun(string input)
    {
        Stack<string> result = rooms.DepthFirstSearch(input);

        Output.text = "DFS result for " + input + ":\n\n";
        while (result.Count >= 1)
            Output.text += result.Pop() + "\n";
    }

    //Button service functions
    private void Button1Service()
    {
        TestRun(null);
    }

    private void Button2Service()
    {
        TestRun("Boutique");
    }

    private void Button3Service()
    {
        TestRun("Shoes");
    }

    private void Button4Service()
    {
        TestRun("Video Game Store");
    }
}
