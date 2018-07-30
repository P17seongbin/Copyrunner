using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour {

    bool Is_Toggled;
    int Spell_ID, Attack_Spell_ID;

    public void Spell_Load(bool Is_AttackSpell, int _Spell_ID)
    {

    }

    private void Spell_UnLoad()
    {

    }

    // Use this for initialization
    void Start () {
        Is_Toggled = false;
        Spell_ID = 0;
        Attack_Spell_ID = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Spell_C()
    {
        

        if(transform.Find("Unit_D(Clone)(Clone)").GetComponent<Unit>().Is_Fixed == true)
            transform.Find("Unit_D(Clone)(Clone)").GetComponent<Unit>().Health -= 12;
        else
            transform.Find("Unit_D(Clone)(Clone)").GetComponent<Unit>().Health -= 2;

        if (transform.Find("Unit_E(Clone)(Clone)").GetComponent<Unit>().Is_Fixed == true)
            transform.Find("Unit_E(Clone)(Clone)").GetComponent<Unit>().Health -= 15;
        else
            transform.Find("Unit_E(Clone)(Clone)").GetComponent<Unit>().Health -= 3;

    }
}
