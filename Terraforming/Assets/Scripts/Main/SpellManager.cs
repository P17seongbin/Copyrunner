using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour {

    bool Is_Toggled;
    int Spell_ID, Attack_Spell_ID;

    public void Spell_Load(bool Is_AttackSpell, int Spell_ID)
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
}
