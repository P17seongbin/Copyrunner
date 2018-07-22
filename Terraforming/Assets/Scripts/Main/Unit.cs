using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : TimeManager
{
    public float Max_Health, Health;// 최대 체력수치/현재 체력수치를 나타냅니다, Unity 편집기에서 각 Unit별로 고유의 값을 지정합니다.
    public float Cost; // 이 Unit을 소환하는데 필요한 자원(에너지) 의 양입니다.Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.
    public float Speed; //각 Unit의 고유 속도입니다, 단위는 Unity Meter입니다.Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.
    public float Summon_Time; //각 Unit의 고유 소환시간입니다. 단위는 초입니다. Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.
    public float Attack_Speed; //각 Unit이 공격을 1회 하는데 걸리는 시간을 의미합니다.Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.
    public BoxCollider2D Hitbox, Attack_Range;//Unit의 피격 Hitbox와 공격범위 Hitbox입니다. Editor에서 할당받습니다.
    public int Team; //이 Unit이 속해있는 팀을 나타냅니다. 여기서 Team은 1 또는 -1입니다.



    [SerializeField] private float Unit_FrameRate;//TimeManager의 FrameRate를 Inspector에서 편집할 수 있게 만들어줍니다.

    private Vector3 Cur_Env; //매 Frame마다 GameManager에게 현재 환경 변수를 받아오는 것을 저장할 임시 변수입니다.
    private bool Is_Moveable; //현재 Unit.cs가 붙어 있는 Object가 움직일 수 있는지를 나타냅니다. 기본값은 True이며, 앞에 공격할 수 있는 다른 Object가 있거나 health가 0보다 작아지면 False로 변경됩니다.
    private GameObject HQ; //Unit을 소환한 HeadQuarter GameObject를 저장합니다.
    private GameManager GM_Script; //GameManager Object에 붙어있는 GamemManager.cs를 나타냅니다.
    private int ID; //이 Unit의 번호를 나타냅니다. (할당된 슬롯 순서)
    private bool Is_Paused = false;
    [SerializeField] private float ATK, DEF; //공격력, 방어력
    private float Last_AttackTime;//마지막으로 공격한 시간을 저장합니다
    private List<Transform> Enemies = new List<Transform>();//히트박스에 들어온 적들을 나타냅니다. 


    void Start()
    {
        TimeScale = 1f;
        FrameRate = Unit_FrameRate;

        Last_AttackTime = Time.fixedTime;
        LastUpdateTime = Time.fixedTime;
        GM_Script = GameObject.Find("GameManager").GetComponent<GameManager>();
        Cur_Env = new Vector3 (0, 0, 0);
        Is_Moveable = true;
        Is_Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Is_Paused)
            {
                Debug.Log("Unpaused");
                TimeScale = 1;
                Is_Paused = false;
            }
            else
            {
                Debug.Log("Paused");
                Is_Paused = true;
                TimeScale = 0;
            }
        }

        if (Refresh_TimeCheck())
        {
            Is_Moveable = true;
            Check_Dead();
            Get_RGBValue();

            if (Health < 0)
            {
                Is_Moveable = false;
            }
            else if (Enemies.Count != 0)//(공격 가능한 Object가 사정거리 내에 있으면)
            {
                Is_Moveable = false;
            }
            Debug.Log(Enemies.Count);
            Move();

            if(Time.fixedTime - Last_AttackTime >= Attack_Speed)//마지막 공격 후 공격주기만큼의 시간이 지났으면 공격을 시도한다.
            {
                Last_AttackTime = Time.fixedTime;
                Attack();
            }
        }
    }

    public void Init(int _Team, int _ID, GameObject HeadQuarter)
    {
        Team = _Team; //팀의 정보를 Unit에게 넘겨줍니다. 여기서 Team은 1 또는 -1입니다.
        ID = _ID; //Unit의 번호를 Unit에게 넘겨줍니다.
        HQ = HeadQuarter; //Unit을 소환한 HeadQuarter GameObject를 Unit에게 넘겨줍니다.
    }

    private void Attack()
    {
        if (Enemies.Count == 0)//때릴놈이 없네
            return;

        //예시로 짠 코드는 가장 가까운 적의 Hit(피격) 함수를 호출하는 것이며, 여기 코드는 기획서에 맞게 수정하셔야 합니다.

        GameObject Target= null;//단일 대상 공격
        //List<GameObject> Target_List; //다중 공격
        float dist = 300000f;

        //가장 가까운 적을 찾아서 Target에 저장합니다.
        foreach (Transform ranged in Enemies)
        {
            if (!ranged.gameObject.activeSelf)
            {
                Enemies.Remove(ranged);
                continue;
            }

            if (ranged.gameObject.GetComponent<Unit>().Team != Team)//적이넹
            {
                float temp_dist = Vector3.Distance(ranged.position, transform.position);
                if (temp_dist < dist)
                {
                    Target = ranged.gameObject;
                    dist = temp_dist;
                }
            }
        }
        if (Target == null)//단일 대상 공격일때 공격 대상이 없는가? (다중 대상이면 다른 조건을 넣어야 합니다)
            return;
        else
        {
            Target.GetComponent<Unit>().Hit(ATK);
        }
    }

    public void Hit(float damage)//피격
    {
        float true_damage = 10f * damage / (10f + DEF);
        Health -= true_damage;
    }

    public float Get_Cost()
    {
        return Cost;
    }
    public float Get_SummonTime()
    {
        return Summon_Time;
    }

    private void Move()
    {
        if (Is_Moveable) //움직일 수 있는지를 판정합니다.
        {
            transform.position = transform.position + new Vector3(1 * Team, 0, 0) * Time.deltaTime * Speed; //Unit을 Team의 여부에 따라 다르게 움직입니다. 여기서 Team은 1 또는 -1입니다.
        }
    }

    private void Check_Dead()
    {
        if (Health < 0) //Unit이 죽었는지를 판정합니다.
        {

        }
    }
    private void Get_RGBValue()
    {
        Cur_Env = GM_Script.Get_RGBValue();
    }
    /*
    IEnumerator Dead_Anim()
    {

    }
    */
    
    public void Add_Enemy(Transform Enemy)
    {
        if (!Enemies.Contains(Enemy))
            Enemies.Add(Enemy);
    }
    public void Remove_Enemy(Transform Enemy)
    {
        if (Enemies.Contains(Enemy))
            Enemies.Remove(Enemy);
    }


}
