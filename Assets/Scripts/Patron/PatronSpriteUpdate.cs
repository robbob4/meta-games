// ------------------------ PatronSpriteUpdate.cs ------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 19, 2016
// Modified - May 12, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a sprite animation behavior script. 
// Keeps track of facing, and reads change in position to determine primary 
// direction.
// ----------------------------------------------------------------------------
// Notes - None.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class PatronSpriteUpdate : MonoBehaviour
{
    #region Sprite variables
    enum Facing
    {
        Unknown = 0,
        South = 1,
        West = 2,
        East = 3,
        North = 4
    }

    private Animator animateComp = null;
    private Facing currentFacing;
    private Vector3 oldPos;
    #endregion

    // Use this for initialization
    void Start()
    {
        animateComp = GetComponent<Animator>();
        if (animateComp == null)
            Debug.LogError("Animator not found for " + this + ".");

        currentFacing = Facing.South;
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //determine delta
        float xAxis = transform.position.x - oldPos.x;
        //float yAxis = transform.position.y - oldPos.y;
        oldPos = transform.position;

        //animate the sprite
        //updateSprite(xAxis, yAxis);
        updateSprite(xAxis, 0);
    }

    #region Sprite functions
    //update the sprite facing, and animation speed
    private void updateSprite(float x, float y)
    {
        if (animateComp == null)
            return;

        Facing newFacing = Facing.Unknown;

        //set animation speed
        animateComp.SetFloat("Horizontal", x);
        //animateComp.SetFloat("Vertical", y);

        //determine strongest directional
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0)
                newFacing = Facing.East;
            else
                newFacing = Facing.West;
        }
        //else if (Mathf.Abs(y) > Mathf.Abs(x))
        //{
        //    if (y > 0)
        //        newFacing = Facing.North;
        //    else
        //        newFacing = Facing.South;
        //}

        if (newFacing != currentFacing)
        {
            //if (newFacing == Facing.North || newFacing == Facing.South)
            //    animateComp.SetBool("VerticalFacing", true);
            //else
            //    animateComp.SetBool("VerticalFacing", false);

            animateComp.SetTrigger("NewFacing");
            currentFacing = newFacing;
        }
    }
    #endregion
}