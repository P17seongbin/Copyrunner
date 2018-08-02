using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : TimeManager
{
    public float Max_Health, Health;// 최대 체력수치/현재 체력수치를 나타냅니다, Unity 편집기에서 각 Unit별로 고유의 값을 지정합니다.
    public float Cost, Ori_Cost, Fixed_Cost; // 이 Unit을 소환하는데 필요한 자원(에너지) 의 양입니다.Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.
    public float Speed, Ori_Speed, Fixed_Speed; //각 Unit의 고유 속도입니다, 단위는 Unity Meter입니다.Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.
    public float Summon_Time, Ori_Summon_Time, Fixed_Summon_Time; //각 Unit의 고유 소환시간입니다. 단위는 초입니다. Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.

    //public int[] Attack_Num; //단위공격주기가 지난 후 공격할 지 말지를 결정하는 배열입니다.

    public int Effect_Env; //각 유닛이 영향을 받는 환경의 종류를 나타낸다. 0(R), 1(G), 2(B) 중 하나의 값을 가진다. 
    public float Affect_Env_Period; ////각 유닛이 환경에 영향을 주는 주기 
    public float Max_Effect_Env, Min_Effect_Env; //각 유닛이 환경에 영향을 받을 때, 참조하는 조건
    public Vector3 Delta_Env;//각 유닛이 환경에 영향을 줄 때, 환경 변수값의 변화량
    public BoxCollider2D Hitbox, Attack_Range;//Unit의 피격 Hitbox와 공격범위 Hitbox입니다. Editor에서 할당받습니다
    public float Ori_Attack_Range, Fixed_Attack_Range;
    public int Team; //이 Unit이 속해있는 팀을 나타냅니다. 여기서 Team은 1 또는 -1입니다.
    //public bool Form_Change; //환경에 영향을 받을 때 공격 방식이 변화하는가? 각 유닛마다 true나 false 사전에 설정
    public int ID; //이 Unit의 번호를 나타냅니다. (할당된 슬롯 순서)
                   //Ori : 고유스탯, Fixed : 환경에 의해 변화된 스탯

    private float Delta_AttackTime;
    private bool Is_Moveable; //현재 Unit.cs가 붙어 있는 Object가 움직일 수 있는지를 나타냅니다. 기본값은 True이며, 앞에 공격할 수 있는 다른 Object가 있거나 health가 0보다 작아지면 False로 변경됩니다.
    private bool Is_Attack; //현재 Unit.cs가 붙어 있는 Object가 공격하고 있는지를 나타냅니다. 기본값은 False이며, 지금 공격하고 있는 다른 Object가 있으면 True로 변경됩니다.
    private bool Is_Dead; //현재 Unit.cs가 붙어 있는 Object가 죽었는지를 나타냅니다. 기본값은 False이며, 이 Unit이 죽으면 True로 변경됩니다.
    public bool Is_Fixed; //현재 Unit.cs가 붙어 있는 Object가 환경에 의해 변화되었는지를 나타냅니다. Ori 상태일 경우 False이며, Fixed 상태일 경우 True입니다.
    private bool Is_Paused = false;
    private GameObject HQ; //Unit을 소환한 HeadQuarter GameObject를 저장합니다.
    private GameManager GM_Script; //GameManager Object에 붙어있는 GamemManager.cs를 나타냅니다.
    private List<Transform> Enemies = new List<Transform>();//히트박스에 들어온 적들을 나타냅니다. 

    //각각 걷는 / 공격하는 / 죽는 애니메이션입니다
    private List<Sprite> WalkingFrame;
    private List<Sprite> AttackFrame;
    private List<Sprite> DeadFrame;
    Coroutine Anim;
    [SerializeField] private int WalkingFrameNumber=3, AttackFrameNumber=3, DeadFrameNumber=3;
    private float DeadAnimDeltaTime = 0.5f;
    private SpriteRenderer This_Renderer;

    [SerializeField] private char Unit_Type;
    [SerializeField] private float Attack_Speed; //각 Unit이 공격을 하는데 걸리는 시간입니다. 
    [SerializeField] private float Unit_FrameRate;//TimeManager의 FrameRate를 Inspector에서 편집할 수 있게 만들어줍니다
    [SerializeField] private Vector3 Cur_Env; //매 Frame마다 GameManager에게 현재 환경 변수를 받아오는 것을 저장할 임시 변수입니다.
    [SerializeField] private float ATK, DEF; //최종 공격력 & 방어력
    [SerializeField] private float Ori_ATK, Fixed_ATK, Ori_DEF, Fixed_DEF;//환경 변수에 영향을 받을 때 / 안받을 때 공격력 & 방어력

    void Start()
    {
        This_Renderer = gameObject.GetComponent<SpriteRenderer>();
        Image_Load();
        TimeScale = 1f;
        FrameRate = Unit_FrameRate;

        WalkingFrameNumber = 2;
        AttackFrameNumber = 3;
        DeadFrameNumber = 3;

        Attack_Speed = 0.5f;
        DeltaTime = 0f;
        Delta_AttackTime = 0f;
        GM_Script = GameObject.Find("GameManager").GetComponent<GameManager>();
        Cur_Env = new Vector3(0, 0, 0);

        Is_Moveable = true;
        Is_Attack = false;
        Is_Dead = false;
        Is_Paused = false;

        InvokeRepeating("Change_Env", Affect_Env_Period, Affect_Env_Period); //일정 주기마다 환경값을 바꾼다

        Stat_Update(); //시작할 때 딱 한 번 스텟을 업데이트 시켜준다.

        Anim = StartCoroutine(Animate());
      //  StartCoroutine("Attack_Time"); //공격
    }

    void Image_Load()
    {
        WalkingFrame = new List<Sprite>();
        DeadFrame = new List<Sprite>();
        AttackFrame = new List<Sprite>();
   //     try
   //      {
            for (int i = 1; i <= WalkingFrameNumber; i++)
            {
                WalkingFrame.Add(Resources.Load<Sprite>("Unit_Image/Unit_" + Unit_Type + "/Unit_" + Unit_Type + "_walking_" + i.ToString()));
            }
            for (int i = 1; i <= DeadFrameNumber; i++)
            {
                DeadFrame.Add(Resources.Load<Sprite>("Unit_Image/Unit_" + Unit_Type + "/Unit_" + Unit_Type + "_die_" + i.ToString()));
            }
            for (int i = 1; i <= AttackFrameNumber; i++)
            {
                AttackFrame.Add(Resources.Load<Sprite>("Unit_Image/Unit_" + Unit_Type + "/Unit_" + Unit_Type + "_attack_" + i.ToString()));
            }
       // }
     //   catch
      //  {
      //      Debug.Log("Sprite for Unit " + Unit_Type + "doesn't exist!");
      //  }
    }

    IEnumerator Animate()//애니메이션 스크립트
    {
        while(!Is_Dead)
        {
            for(int i=0;i<AttackFrameNumber;i++)
            {
                if (!Is_Attack)
                    break;
                This_Renderer.sprite = AttackFrame[i];
                yield return new WaitForSeconds(Attack_Speed/AttackFrameNumber);
            }
            for(int i=0;i<WalkingFrameNumber;i++)
            {
                if (Is_Attack)
                    break;
                This_Renderer.sprite = WalkingFrame[i];
                yield return new WaitForSeconds(Speed/WalkingFrameNumber/2f);
            }
        }
    }
    IEnumerator Dead_Anim()//사망 애니메이션 및 비활성화 처리
    {
        while (Is_Dead)
        {
            for (int i = 0; i < DeadFrameNumber; i++)
            {
                This_Renderer.sprite = DeadFrame[i];
                yield return new WaitForSeconds(DeadAnimDeltaTime);
            }
            break;
        }

        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Is_Dead)//죽은것이 판정되면 AI를 정지한다
            return;

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

        //현재 일시정지 중이 아니면 흐른 시간을 기록합니다.(DeltaTime값을 1프레임 만큼의 시간만큼 더해줍니다)
        if (!Is_Paused)
            DeltaTime += Time.fixedDeltaTime;
        //현재 공격 중이며, 일시정지 중이 아니면 흐른 시간을 기록합니다(위와 동일합니다)
        if (!Is_Paused && Is_Attack)
            Delta_AttackTime += Time.fixedDeltaTime;

        //DeltaTime이 FrameRate보다 더 큰가?
        if (Refresh_TimeCheck())
        {

            Check_Dead();
            Get_RGBValue();
            Stat_Update(); //환경에 따른 스탯 변화 업데이트


            Move();

            //마지막 공격으로부터 지난 시간이 공격 주기보다 크면 Attack을 1회 호출합니다.
            if (Delta_AttackTime >= Attack_Speed)
            {
                Delta_AttackTime = 0f;
                Attack();
                //특정 상황에서 공격 주기가 달라지는 Unit D의 공격 패턴을 구현하기 위해 특수한 조건을 삽입했습니다.
                if (Unit_Type == 'D' || Unit_Type == 'd')
                {
                    if (6 <= Cur_Env[Effect_Env] && Cur_Env[Effect_Env] <= 8)
                    {
                        Invoke("Attack", 0.15f);
                    }
                }

            }

        }
    }

    private void Stat_Update() //update 함수에 의해 호출됨
    {
        if (Min_Effect_Env <= Cur_Env[Effect_Env] && Cur_Env[Effect_Env] <= Max_Effect_Env)  //유닛이 환경에 영향받는 조건을 만족할 때
        {
            Is_Fixed = true;
            Cost = Fixed_Cost;
            Summon_Time = Fixed_Summon_Time;
            ATK = Fixed_ATK;
            DEF = Fixed_DEF;
            Speed = Fixed_Speed;
            Attack_Range.size = new Vector2(Fixed_Attack_Range, 2.0f); //각 스탯들을 전부 fixed 값으로 교체
            Attack_Range.offset = new Vector2( (Attack_Range.size.x / 2f) + 1.2f,0); //사거리 히트박스를 유닛의 위치에 맞게 재배치
        }
        else 
        {
            Is_Fixed = false;
            Cost = Ori_Cost;
            Summon_Time = Ori_Summon_Time;
            ATK = Ori_ATK;
            DEF = Ori_DEF;
            Speed = Ori_Speed;
            Attack_Range.size = new Vector2(Ori_Attack_Range, 2.0f);
            Attack_Range.offset = new Vector2(0, 0); //각 스탯들을 전부 origin 값으로 교체
            Attack_Range.offset = new Vector2((Attack_Range.size.x / 2f) + 1.2f, 0);//사거리 히트박스를 유닛의 위치에 맞게 재배치

        }

    }

    public void Change_Env() //유닛에 의한 환경 변화
    {
        //GameManager gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //gamemanager.Cur_Env[Affect_Env] = Cur_Env[Affect_Env] + Delta_Env;
        GM_Script.Change_RGBValue(Delta_Env);
    }

    public void Init(int _Team, GameObject HeadQuarter)
    {
        Team = _Team; //팀의 정보를 Unit에게 넘겨줍니다. 여기서 Team은 1 또는 -1입니다.
        HQ = HeadQuarter; //Unit을 소환한 HeadQuarter GameObject를 Unit에게 넘겨줍니다.
    }
    /*
    IEnumerator Attack_Time()
    {

        while(!Is_Dead)
        {
            Attack();


            yield return new WaitForSeconds(Attack_Speed);
            
            for (int i = 0; i < Attack_Num.Length; i++) //Attack_Num[i] 안의 수가 0일 때는 공격불가, 1, 2, 3일 때는 밑에 주석에 나온다.
            {
                if (Attack_Num[i] == 3) //Ori든 Fixed든 상관없을 때
                {
                    Attack();
                }
                else if (Attack_Num[i] == 1 && Is_Fixed == false) //Ori 상태일 때의 공격주기
                {
                    Attack();
                }
                else if (Attack_Num[i] == 2 && Is_Fixed == true) //Fixed 상태일 때의 공격주기
                {
                    Attack();
                }

                yield return new WaitForSeconds(Attack_Speed);
            }
            
        }
    }
*/
    private void Attack()
    {
        if (Enemies.Count == 0)//때릴놈이 없네
            return;

        //예시로 짠 코드는 가장 가까운 적의 Hit(피격) 함수를 호출하는 것이며, 여기 코드는 기획서에 맞게 수정하셔야 합니다.

        GameObject Target = null;//단일 대상 공격
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

            if (ranged.gameObject.tag == "Unit")
            {
                if (ranged.gameObject.GetComponent<Unit>().Team != Team) //적 Unit이넹
                {
                    float temp_dist = Vector3.Distance(ranged.position, transform.position);
                    if (temp_dist < dist)
                    {
                        Target = ranged.gameObject;
                        dist = temp_dist;
                    }
                }
            }
            else if (ranged.gameObject.tag == "HQ")
            {
                if (ranged.gameObject.GetComponent<HeadQuarter>().Team != Team) //적 HQ이넹
                {
                    float temp_dist = Vector3.Distance(ranged.position, transform.position);
                    if (temp_dist < dist)
                    {
                        Target = ranged.gameObject;
                        dist = temp_dist;
                    }
                }
            }
        }
        if (Target == null)//단일 대상 공격일때 공격 대상이 없는가? (다중 대상이면 다른 조건을 넣어야 합니다)
            return;
        else
        {
            if (Target.tag == "Unit")
            {
                Target.GetComponent<Unit>().Hit(ATK);
            }
            else if (Target.tag == "HQ")
            {
                Target.GetComponent<HeadQuarter>().Hit(ATK);
            }
        }
    }

    public void Hit(float ATK)//피격
    {
        float true_damage = 10f * ATK / (10f + DEF);
        Health -= true_damage;
    }
    public void Pierce_Hit(float ATK)//고정데미지
    {
        Health -= ATK;
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
        if (Health <= 0) //Unit이 죽었는지를 판정합니다.
        {
            Is_Moveable = false;
            Is_Attack = false;
            Is_Dead = true;
            HQ.GetComponent<HeadQuarter>().Unit_Dead(ID);

            StopCoroutine(Anim);

            StartCoroutine(Dead_Anim());
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
        if(Enemies.Count == 0)
        {
            StopCoroutine(Anim);
            Is_Moveable = false;
            Is_Attack = true;
            Anim = StartCoroutine(Animate());
        }
        if (!Enemies.Contains(Enemy))
            Enemies.Add(Enemy);
    }
    public void Remove_Enemy(Transform Enemy)
    {
        if(Enemies.Count == 1)
        {
            StopCoroutine(Anim);
            Is_Moveable = true;
            Is_Attack = false;
            Anim = StartCoroutine(Animate());
        }
        if (Enemies.Contains(Enemy))
            Enemies.Remove(Enemy);
    }

    public char Get_Type()
    {
        return Unit_Type;
    }
}
