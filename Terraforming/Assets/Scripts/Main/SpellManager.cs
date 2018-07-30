using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour {

    public int Team;
    bool Is_Toggled;
    int Spell_ID, Attack_Spell_ID;
    GameManager GM_Script;
    HeadQuarter HQ_Script; //HeadQuarter Object 안에 SpellManager가 들어갑니다.

    public void Spell_Load(bool Is_AttackSpell, int _Spell_ID)
    {
        HQ_Script.Toggle_Is_Casting();
        Is_Toggled = true;

        if (Is_AttackSpell)
        {
            Attack_Spell_ID = _Spell_ID;
        }
        else
        {
            Spell_ID = _Spell_ID;
        }
    }

    private void Spell_UnLoad()
    {
        HQ_Script.Toggle_Is_Casting();
        Is_Toggled = false;
    }

    // Use this for initialization
    void Start () {

        GM_Script = GameObject.Find("GameManager").GetComponent<GameManager>(); //환경을 바꾸기 위해서 GameManager.cs 안의 Change_RGBValue 함수를 쓰기 위해 가지고 온 컴포넌트입니다.
        HQ_Script = GetComponent<HeadQuarter>(); //자신의 HQ의 정보를 받아오기 위해 씁니다.
        Is_Toggled = false; //현재 주문을 사용하고 있는지를 나타내는 변수입니다.
        Spell_ID = 0; //Spell_ID의 값에 따라 시전되는 Spell이 달라집니다. 기본값인 0의 의미는 Spell이 시전되지 않는다라는 의미입니다.
        Attack_Spell_ID = 0; //Attack_Spell_ID의 값에 따라 시전되는 Attack Spell이 달라집니다. 기본값인 0의 의미는 Spell이 시전되지 않는다라는 의미입니다.
    }

	// Update is called once per frame
	void Update () {

        if(Is_Toggled) //주문이 시전되면
        {
            if (Spell_ID != 0) //나중에 주문이 여러개가 된다면 변경할 것입니다.
            {
                Spell_C();
                Debug.Log("Spell Casted!");
                Spell_UnLoad();
            }
            if (Attack_Spell_ID != 0)
            {
                Attack_Spell_A();
                Spell_UnLoad();
            }
        }
	}

    void Spell_C() //Spell_ID는 3
    {
        //아직 0은 Creature D, 1은 Creature E입니다.
        GM_Script.Change_RGBValue(new Vector3(0, HQ_Script.Get_Unit_Count()[0], HQ_Script.Get_Unit_Count()[1] * 2));

        if (GameObject.Find("Unit_D(Clone)(Clone)").GetComponent<Unit>().Is_Fixed == true)
            GameObject.Find("Unit_D(Clone)(Clone)").GetComponent<Unit>().Health -= 12;
        else
            GameObject.Find("Unit_D(Clone)(Clone)").GetComponent<Unit>().Health -= 2;

        if (GameObject.Find("Unit_E(Clone)(Clone)").GetComponent<Unit>().Is_Fixed == true)
            GameObject.Find("Unit_E(Clone)(Clone)").GetComponent<Unit>().Health -= 15;
        else
            GameObject.Find("Unit_E(Clone)(Clone)").GetComponent<Unit>().Health -= 3;
    }

    void Attack_Spell_A()
    {

    }
}
