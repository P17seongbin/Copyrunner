using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadQuarter : MonoBehaviour {

    public int Team;
    public int MAX_UNIT_VARIATION=3; //플레이어가 가질 수 있는 유닛의 총 개수를 의미합니다, Defalut값은 3입니다.

    bool Is_Casting; //주문을 시전하고 있는지 여부를 나타냅니다.Default값은 False입니다.
    bool Is_Paused; //현재 게임이 일시정지 되었는지를 나타냅니다.Default값은 False입니다.
    float Health; //HQ의 현재 체력을 나타냅니다.
    float Resource, Max_Resource; //HQ가 현재 소유한 자원의 수와 최대 자원의 수를 나타냅니다.
    int[] Unit_Count; //종류별 현재 Unit의 수를 나타냅니다.Summon_Unit() 함수를 호출할 때 마다 각 종류에 해당하는 값이 1씩 증가하며, Removed() 함수가 호출될 때 마다 각 Unit 종류에 해당하는 값이 1씩 감소합니다.
    float[] Unit_Cost;
    float[] Unit_Summon_Time; //플레이어가 선택한 Unit의 Cost와 Summon_Time을 저장합니다. Object Pooling 과정에서 저장합니다.
    GameObject[] Unit_Template; //플레이어가 선택한 Unit의 Prefab을 저장합니다, Object Pooling 과정에서 이 Prefab을 활용하여 진행합니다.
    int[] Unit_Queue = new int[5]; //소환 대기열에 들어있는 Unit의 번호를 나타냅니다.
    



    //이 함수는 건드리지 마세요
    public GameObject Summon_Unit(int ID)
    {
        if (0 <= ID && ID < MAX_UNIT_VARIATION)
        {
            Unit_Count[ID]++;
            GameObject res = Instantiate(Unit_Template[ID]);
            return res;
        }
        else
            return null;
    }

    public void Hit(float damage)
    {
        //HQ는 크리쳐의 공격력만큼 그대로 데미지를 입는다
        Health = Health - damage;
    }
    public float HQ_Health()
    {
        return Health;
    }
    
    //이 함수는 건드리지 마세요
    public void Pause_Toggle(bool Is_Pause)
    {

    }

    public void Finish_Game()
    {

    }

    public int[] Get_Unit_Count()
    {
        return Unit_Count;
    }
    public void Unit_Dead(int ID)
    {
        //안전장치
        if (0 <= ID && ID < MAX_UNIT_VARIATION)
            Unit_Count[ID] = Unit_Count[ID] - 1;
    }
    public void Toggle_Is_Casting()
    {
        Is_Casting = !Is_Casting;
    }
    //이 함수는 아직 건드리지 마세요
    private void Object_Pooling()
    {

    }
    // Use this for initialization
    void Start()
    {
        Is_Casting = true;
        Unit_Count = new int[MAX_UNIT_VARIATION]; //종류별 현재 Unit의 수를 나타냅니다.Summon_Unit() 함수를 호출할 때 마다 각 종류에 해당하는 값이 1씩 증가하며, Removed() 함수가 호출될 때 마다 각 Unit 종류에 해당하는 값이 1씩 감소합니다.
        Unit_Cost = new float[MAX_UNIT_VARIATION];
        Unit_Summon_Time = new float[MAX_UNIT_VARIATION]; //플레이어가 선택한 Unit의 Cost와 Summon_Time을 저장합니다. Object Pooling 과정에서 저장합니다.
        Unit_Template = new GameObject[MAX_UNIT_VARIATION]; //플레이어가 선택한 Unit의 Prefab을 저장합니다, Object Pooling 과정에서 이 Prefab을 활용하여 진행합니다.
        for (int i = 0; i < MAX_UNIT_VARIATION; i++)//초기화
            Unit_Template[i] = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Resource > Max_Resource)
        {
            Resource = Max_Resource;
        }
		else if(Resource < Max_Resource)
        {
            Resource = Resource + Time.deltaTime;
        }
	}
}
