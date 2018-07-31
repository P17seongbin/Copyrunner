using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour {

    public int Team;
    bool Is_Toggled;
    int Spell_ID, Attack_Spell_ID;

    private float Spell_C_FinishedTime; //마지막으로 스펠 C가 끝난 시간을 저장합니다.

    GameManager GM_Script;
    HeadQuarter HQ_Script; //HeadQuarter Object 안에 SpellManager가 들어갑니다.

    public void Spell_Load(bool Is_AttackSpell, int _Spell_ID)
    {
        //HQ_Script.Toggle_Is_Casting();

        if (!Is_Toggled) //안전장치
        {
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
    }

    private void Spell_UnLoad()
    {
        // HQ_Script.Toggle_Is_Casting();
        if (Is_Toggled) //안전장치
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
            if (Spell_ID != 0 && Time.fixedTime - Spell_C_FinishedTime > 3f) //나중에 주문이 여러개가 된다면 변경할 것입니다.
            {                                                                //현재 시간과 스펠 C가 끝난 시간을 비교해서 쿨타임보다 많은 시간이 지났을 경우 실행합니다.
                Spell_C();
                Debug.Log("Spell Casted!");
            }
            if (Attack_Spell_ID != 0)
            {
                Attack_Spell_A();
            }

            Spell_UnLoad();
        }
	}

    void Spell_C() //Spell_ID는 3
    {
        if (GetComponent<HeadQuarter>().Resource >= 3) //본진의 에너지가 스펠 코스트보다 높을 때
        {
            GetComponent<HeadQuarter>().Resource -= 3; //본진의 에너지 자원 소모


            GameObject[] Unit_List = GameObject.FindGameObjectsWithTag("Unit");//필드위에 있는 모든 유닛을 받아옵니다.

            int DCount=0, ECount=0;
            foreach (GameObject Unit in Unit_List)//특정 배열(리스트)컨테이너 내 모든 원소를 순회하는 for문의 변종
            {
                if (Unit.activeSelf)
                {
                    Unit Unit_Script = Unit.GetComponent<Unit>();//여러번 참조할 Unit Component를 저장해서 약간 최적화
                    char Unit_Type = Unit_Script.Get_Type();//Get_Type()함수를 새로 만들고, 그걸로 Unit의 Type를 받아옵니다.

                    if (Unit_Type == 'D')
                    {
                        DCount++;
                        Unit_Script.Health -= Unit_Script.Is_Fixed ? 12 : 2;//삼항연산자, (조건문) ? (True일때 값) : (False일떄 값 입니다.
                    }
                    else if (Unit_Type == 'E')
                    {
                        ECount++;
                        Unit_Script.Health -= Unit_Script.Is_Fixed ? 15 : 3;
                    }
                }
            }
            GM_Script.Change_RGBValue(new Vector3(0, DCount, ECount));

            //아직 0은 Creature D, 1은 Creature E입니다.
            //GM_Script.Change_RGBValue(new Vector3(0, HQ_Script.Get_Unit_Count()[0], HQ_Script.Get_Unit_Count()[1] * 2));
            /*
            if (GameObject.Find("Unit_D(Clone)(Clone)").GetComponent<Unit>().Is_Fixed == true) //유닛이 환경의 영향을 받을 때
                GameObject.Find("Unit_D(Clone)(Clone)").GetComponent<Unit>().Health -= 12; //스펠의 효과에 맞게 유닛의 HP 경감
            else
                GameObject.Find("Unit_D(Clone)(Clone)").GetComponent<Unit>().Health -= 2;

            if (GameObject.Find("Unit_E(Clone)(Clone)").GetComponent<Unit>().Is_Fixed == true) //유닛이 환경의 영향을 받을 때
                GameObject.Find("Unit_E(Clone)(Clone)").GetComponent<Unit>().Health -= 15;
            else
                GameObject.Find("Unit_E(Clone)(Clone)").GetComponent<Unit>().Health -= 3;
                */
            Spell_C_FinishedTime = Time.fixedTime; //마지막으로 스펠 C가 끝난 시간을 저장합니다.
        }
    }

    void Attack_Spell_A()
    {

    }
}
