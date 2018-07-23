using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public Unit Parent;
    private Unit collidedUnit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Unit")
        {
            collidedUnit = collision.GetComponent<Unit>();

            if (collidedUnit.Team != Parent.Team)
            {
                Parent.Add_Enemy(collision.transform);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Unit")
        {
            collidedUnit = collision.GetComponent<Unit>();

            if (collidedUnit.Team != Parent.Team)
            {
                Parent.Remove_Enemy(collision.transform);
            }
        }
    }

}
