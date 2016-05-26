using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElevatorCar : MonoBehaviour {

    [SerializeField] private int MaxCapacity = 1;
    [SerializeField] private float WaitTime = 3.0f;
    [SerializeField] private float TimePerFloor = 1.5f;

    private static GameTime gameTimer = null;

    enum Status
    {
        idle,
        called,
        waiting,
        ascending,
        descending
    }

    struct PatronNode
    {
        public Patron thePatron;
        public int destination;
    }

    private List<PatronNode> patrons;

    private float Timer;
    private Status CurrentState;
    private bool IsWaiting;
    private int CurrentFloor;
    private int NextFloor;

    void Awake ()
    {
        gameTimer = GameObject.Find("GameManager").GetComponent<GameTime>();
        if (gameTimer == null)
            Debug.LogError("GameTime not found for " + this + ".");
    }

    // Use this for initialization
    void Start ()
    {
        CurrentState = Status.idle;
        Timer = 0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //TODO: Set height based on current floor
        //TODO: Set heihgt of all passengers to current height

        if (!gameTimer.Paused)
        {
            Timer += Time.deltaTime;

            switch (CurrentState)
            {
                case Status.idle:
                case Status.called:
                    if (CurrentFloor > NextFloor)
                    {
                        MoveCar(false);
                    }
                    else if (CurrentFloor < NextFloor)
                    {
                        MoveCar(true);
                    }
                    break;
                case Status.waiting:
                    break;
                case Status.ascending:
                    MoveCar(true);
                    break;
                case Status.descending:
                    MoveCar(false);
                    break;
                default:
                    break;
            }
        }
	}

    public void CallToFloor(int dest)
    {
        if (CurrentState == Status.idle)
        {
            CurrentState = Status.called;
            NextFloor = dest;
        }
    }

    public bool IsReady()
    {
        return (CurrentState == Status.idle);
    }

    public bool HasRoom()
    {
        return (patrons.Count != MaxCapacity);
    }

    //temp
    public void SetFloor(int floor)
    {
        CurrentFloor = floor;
    }

    private void MoveCar(bool goUp)
    {
        if (Timer >= TimePerFloor)
        {
            Timer = 0f;

            if (goUp)
            {
                CurrentFloor++;
                for (int i = 0; i < patrons.Count; i++)
                {
                    patrons[i].thePatron.CurrentFloor++;
                }
            }
            else
            {
                CurrentFloor--;
                for (int i = 0; i < patrons.Count; i++)
                {
                    patrons[i].thePatron.CurrentFloor--;
                }
            }

            CheckPassengers(goUp);
        }
    }

    //Return true is patron accepted, return false if car is full
    public bool AcceptPatron (Patron thePatron)
    {
        if (patrons.Count >= MaxCapacity)
        {
            return false;
        }

        PatronNode theNode = new PatronNode();
        theNode.thePatron = thePatron;
        theNode.destination = thePatron.GetNextFloor();
        patrons.Add(theNode);

        CheckPassengers(thePatron.GetNextFloor() > CurrentFloor);

        return true;
    }

    //Call to attach this elevator car to the elevator shaft
    public void InsertShaft(ElevatorMain ElevatorShaft)
    {
        gameObject.transform.parent = ElevatorShaft.transform;
        CurrentFloor = ElevatorShaft.GetHomeFloor();
        transform.position = new Vector3(
            ElevatorShaft.transform.position.x,
            ElevatorShaft.transform.position.y, //TODO: Set y so it's on the correct HomeFloor level.
            ElevatorShaft.transform.position.z);
        
    }

    private void CheckPassengers (bool goUp)
    {
        int tempNextFloor;

        if (goUp)
        {
            tempNextFloor = int.MaxValue;
            CurrentState = Status.ascending;
        }
        else
        {
            tempNextFloor = int.MinValue;
            CurrentState = Status.descending;
        }

        for (int i = patrons.Count -1; i >= 0; i--)
        {
            if (patrons[i].destination == CurrentFloor)
            {
                //TODO: Everything to make sure the patron can continue on
                patrons[i].thePatron.Movement = true;
                patrons.RemoveAt(i);
            }
            else
            {
                if (goUp && patrons[i].destination < tempNextFloor )
                {
                    tempNextFloor = patrons[i].destination;
                }
                else if (!goUp && patrons[i].destination > tempNextFloor)
                {
                    tempNextFloor = patrons[i].destination;
                }
            }
        }

        if (patrons.Count == 0)
        {
            CurrentState = Status.idle;
        }
        else
        {
            NextFloor = tempNextFloor;
        }
    }
}
