﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public Unit Parent;
    private Unit collidedUnit;
    private HeadQuarter collidedHQ;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Unit")
        {
            collidedUnit = collision.gameObject.GetComponent<Unit>();

            if (collidedUnit.Team != Parent.Team)
            {
                Parent.Add_Enemy(collision.transform);
            }
        }
        else if(collision.gameObject.tag == "HQ")
        {
            collidedHQ = collision.gameObject.GetComponent<HeadQuarter>();

            if (collidedHQ.Team != Parent.Team)
            {
                Parent.Add_Enemy(collision.transform);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "Unit")
        {
            collidedUnit = collision.gameObject.GetComponent<Unit>();

            if (collidedUnit.Team != Parent.Team)
            {
                Parent.Remove_Enemy(collision.transform);
            }
        }
        else if (collision.gameObject.tag == "HQ")
        {
            collidedHQ = collision.gameObject.GetComponent<HeadQuarter>();

            if (collidedHQ.Team != Parent.Team)
            {
                Parent.Add_Enemy(collision.transform);
            }
        }
    }

}
