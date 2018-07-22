using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public Unit Parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Unit")
        Parent.Add_Enemy(collision.transform);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Unit")
            Parent.Remove_Enemy(collision.transform);
    }

}
