using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeadQuarter : MonoBehaviour {

    GameManager GM_Script;

    public int Team;
    public int MAX_UNIT_VARIATION=2; //플레이어가 가질 수 있는 유닛의 총 개수를 의미합니다, Defalut값은 3입니다.
    public Text WinText;

    bool Is_Paused; //현재 게임이 일시정지 되었는지를 나타냅니다.Default값은 False입니다.

    [SerializeField] float Health; //HQ의 현재 체력을 나타냅니다.
    [SerializeField] public float Resource; //HQ가 현재 소유한 자원의 수를 나타냅니다.

    int[] Unit_Count; //종류별 현재 Unit의 수를 나타냅니다.Summon_Unit() 함수를 호출할 때 마다 각 종류에 해당하는 값이 1씩 증가하며, Removed() 함수가 호출될 때 마다 각 Unit 종류에 해당하는 값이 1씩 감소합니다.
    float[] Unit_Cost;
   // float[] Unit_Summon_Time; //플레이어가 선택한 Unit의 Cost와 Summon_Time을 저장합니다. Object Pooling 과정에서 저장합니다.
    GameObject[] Unit_Template; //플레이어가 선택한 Unit의 Prefab을 저장합니다, Object Pooling 과정에서 이 Prefab을 활용하여 진행합니다.
    int[] Unit_Queue = new int[5]; //소환 대기열에 들어있는 Unit의 번호를 나타냅니다.



    public bool Summon_Order(int ID)
    {
        if (0 <= ID && ID < MAX_UNIT_VARIATION)
        {
            //현재 소환 가능한지 검사하고 불가능하면 각 경우별로 특수한 행동을 취하게 만들 것.
            if (((Unit_Cost[ID] < Resource) && Is_Paused == false) && (!GetComponent<SummonManager>().Is_Full()))
            {
                Resource = Resource - Unit_Cost[ID]; //서환한 유닛의 코스트 만큼 자원을 줄인다.
                //가능하면 소환 큐에 넣는다.(취소는 불가능하므로 Unit Count + 1)
                GameObject temp = GetComponent<SummonManager>().Enqueue_Unit(Unit_Template[ID]);
                if (temp != null)
                    Unit_Count[ID]++;
                return temp != null;
            }
            else
                return false;
        }
        else
            return false;
    }
    //이 함수는 건드리지 마세요
    /*
    private GameObject Summon_Unit(int ID)
    {
        if (0 <= ID && ID < MAX_UNIT_VARIATION)
        {
            Unit_Count[ID]++;
            GameObject res = Instantiate(Unit_Template[ID]);
            //res의 위치를 HQ의 위치를 통해 계산한다.
            //지금은 기본 위치에 소환하지만 필요하다면 유닛별로 다른 위치를 할당하는 기능을 추가할 것.
            float Unit_XSize = res.GetComponent<BoxCollider2D>().size.x;
            res.transform.position = new Vector3((transform.position.x + (Team * (XSize + Unit_XSize) / 2f)), 0f, 0f);
            res.transform.rotation = Quaternion.Euler(new Vector3(0f, (90f - Team * 90f) , 0f));
            res.GetComponent<Unit>().Init(Team, gameObject);


            return res;


        }
        else
            return null;
    }
    */
    public void Hit(float damage)
    {
        //HQ는 크리쳐의 공격력만큼 그대로 데미지를 입는다
        if (Health > damage)
            Health = Health - damage;
        else
            Health = 0;    //HeadQuarter 의 체력이 음수가 되는 것을 방지
    }
    public float HQ_Health()
    {
        return Health;
    }
    
    //이 함수는 건드리지 마세요
    public void Set_Pause(bool Is_Pause)
    {
        Is_Paused = Is_Pause;
    }

    public void Finish_Game() //게임을 끝내고 결과화면이 나옵니다.
    {
        SceneManager.LoadSceneAsync("Result", LoadSceneMode.Single);
        //GameObject.Find("Game_Result").GetComponent<Information_display>().Result_Display(Team);
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
    // Use this for initialization
    void Start()
    {

        Unit_Count = new int[MAX_UNIT_VARIATION]; //종류별 현재 Unit의 수를 나타냅니다.Summon_Unit() 함수를 호출할 때 마다 각 종류에 해당하는 값이 1씩 증가하며, Removed() 함수가 호출될 때 마다 각 Unit 종류에 해당하는 값이 1씩 감소합니다.
        Unit_Cost = new float[MAX_UNIT_VARIATION];
      //  Unit_Summon_Time = new float[MAX_UNIT_VARIATION]; //플레이어가 선택한 Unit의 Cost와 Summon_Time을 저장합니다. Object Pooling 과정에서 저장합니다.
        Unit_Template = new GameObject[MAX_UNIT_VARIATION]; //플레이어가 선택한 Unit의 Prefab을 저장합니다, Object Pooling 과정에서 이 Prefab을 활용하여 진행합니다.

        Unit_Template = GameObject.Find("GameManager").GetComponent<GameManager>().Get_UnitLIst(Team, MAX_UNIT_VARIATION);
        for (int i = 0; i < MAX_UNIT_VARIATION; i++)//초기화
        {
            try
            {
                Unit_Count[i] = 0;
                Unit_Cost[i] = Unit_Template[i].GetComponent<Unit>().Cost;
            }
            catch
            {
                Debug.Log("Team " + (Team == 1 ? 1 : 2).ToString() + " Selected Less than 3 creatures!");
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
            Resource = Resource + Time.deltaTime;

        if (Health == 0)
            Finish_Game();

    }

    public float Get_Resource() //Information_display.cs 에서 이 함수를 호출한다.
    {
        return Resource;
    }
}
