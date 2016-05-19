using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElevatorMain : MonoBehaviour {

    static int MAX_CAPACITY = 20;

    private int bottomFloor;
    private int topFloor;
    [SerializeField] private float timePerFloor = 1.5F;

    enum Status
    {
        idle,
        returning,
        ascend,
        descend
    }

    struct PatronNode
    {
        public GameObject gObject = null;
        public Patron script = null;
        public Status status = Status.idle;
        public float timer = 0F;
    }

    private List<PatronNode>[] waitlist;

	// Use this for initialization
	void Start ()
    {
        bottomFloor = 1;
        topFloor = -1;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    for (int i=0; i < topFloor - bottomFloor+1; i++)
        {
            for (int j = 0; j < waitlist[i].Count; j++)
            {
                PatronNode temp = waitlist[i][j];
                temp.timer += Time.deltaTime;

                if(temp.timer >= timePerFloor)
                {
                    temp.timer -= timePerFloor;
                    
                    if (temp.status == Status.ascend)
                    {
                        temp.script.CurrentFloor++;
                    }
                    else if (temp.status == Status.ascend)
                    {
                        temp.script.CurrentFloor--;
                    }

                    //TODO: Check if on destination floor
                    // if it is, get off
                }
                
                waitlist[i][j] = temp;
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

    public void EnqueuePatron (GameObject thePatron)
    {
        Patron temp = thePatron.GetComponent<Patron>();
        if (temp == null)
        {
            Debug.LogError("Calling GameObject not Patron.");
            return;
        }

        PatronNode theNode = new PatronNode();
        theNode.gObject = thePatron;
        theNode.script = temp;
        
        // TODO: Determine what floor the destination is on to set if status would be ascending or descending.

        waitlist[theNode.script.CurrentFloor - bottomFloor].Add(theNode);

    }
}
