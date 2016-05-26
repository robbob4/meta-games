using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElevatorMain : MonoBehaviour {

    static int MAX_CAPACITY = 20;

    protected static GlobalGameManager globalGameManager = null;
    protected static GameTime gameTimer = null;
    protected static GameObject spareCar = null;

    private int bottomFloor;
    private int topFloor;
    private int homeFloor;

    struct PatronNode
    {
        public Patron thePatron;
        public bool goingUp;
        public ElevatorCar myCar;    //temp
    }

    private List<PatronNode>[] waitlist;
    private List<ElevatorCar> elevators;

    void Awake()
    {
        #region References
        globalGameManager = GameObject.Find("GameManager").GetComponent<GlobalGameManager>();
        if (globalGameManager == null)
            Debug.LogError("GameManager's GlobalGameManager not found for " + this + ".");

        gameTimer = GameObject.Find("GameManager").GetComponent<GameTime>();
        if (gameTimer == null)
            Debug.LogError("GameTime not found for " + this + ".");

        spareCar = Resources.Load("Prefabs/Transportation/ElevatorCar") as GameObject;
        #endregion
    }
    // Use this for initialization
    void Start ()
    {
        bottomFloor = 1;
        topFloor = -1;
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int k = elevators.Count - 1; k >= 0; k--)
        {
            //HACK
            //if elevator is empty and is idle, it is no longer useful
            if (elevators[k].HasRoom() && elevators[k].IsReady())
            {
                ElevatorCar temp = elevators[k];
                elevators.RemoveAt(k);
                Destroy(temp.gameObject);
            }
        }

        for (int i = 0; i < topFloor - bottomFloor + 1; i++)
        {
            for (int j = waitlist[i].Count - 1; j >= 0; j--)
            {
                //HACK
                //Each passanger gets on the elevator spawned for them
                if (waitlist[i][j].myCar.AcceptPatron(waitlist[i][j].thePatron))
                {
                    waitlist[i].RemoveAt(j);
                }
            }
        }
	}

    public void ChangeSize (int nBottom, int nTop)
    {
        if (nTop <= nBottom)
        {
            return;
        }
        
        bottomFloor = nBottom;
        topFloor = nTop;
        homeFloor = bottomFloor;

        waitlist = null;
        waitlist = new List<PatronNode>[topFloor - bottomFloor + 1];
        for (int i = 0; i < topFloor - bottomFloor+1; i++)
        {
            waitlist[i] = new List<PatronNode>();
        }

        RemakeGraph();
    }

    private void RemakeGraph()
    {
        //TODO: All of this.
    }

    public void EnqueuePatron (Patron thePatron)
    {
        PatronNode theNode = new PatronNode();
        theNode.thePatron = thePatron;
        theNode.goingUp = (thePatron.GetNextFloor() > thePatron.CurrentFloor);
        theNode.myCar = SpawnElevator(thePatron.CurrentFloor);  //temp

        waitlist[theNode.thePatron.CurrentFloor - bottomFloor].Add(theNode);

    }

    //temp, HACK
    private ElevatorCar SpawnElevator(int floor)
    {
        GameObject newElevator = (GameObject)Instantiate(spareCar);
        elevators.Add(newElevator.GetComponent<ElevatorCar>());
        newElevator.GetComponent<ElevatorCar>().InsertShaft(this);
        newElevator.GetComponent<ElevatorCar>().CallToFloor(floor);
        newElevator.GetComponent<ElevatorCar>().SetFloor(floor);

        return newElevator.GetComponent<ElevatorCar>();
    }

    public int GetBottomFloor()
    {
        return bottomFloor;
    }

    public int GetTopFloor()
    {
        return topFloor;
    }

    public int GetHomeFloor()
    {
        return bottomFloor;
    }
}
