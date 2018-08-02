﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Camera MainCamera;
    public Image RGBImage;
    public Text RGBText;

    private Vector3 Cur_Env;
    [SerializeField]//private 변수를 Unity Inspector에서 편집할 수 있게 합니다.
    private float Init_R=5f, Init_G=5f, Init_B=5f;
    [SerializeField] private Vector3 Max_Env, Min_Env;//최대 환경변수 및 최소 환경변수를 나타냅니다, Inspector에서 값을 입력받습니다. 

    private float Cam_Width, Cam_Height;
    private GameObject P1_HQ, P2_HQ;

    private List<GameObject> P1_CreatureList, P2_CreatureList;

    private GameObject[] BG;

    private int Env_L=2, Env_M=7;//배경이 바뀌는 RGB값의 기준


    // Use this for initialization
    void Start()
    {
        P1_CreatureList = new List<GameObject>();
        P2_CreatureList = new List<GameObject>();
        
        Cur_Env = new Vector3(Init_R, Init_G, Init_B);
        Cur_Env = Limit_RGBValue(Cur_Env);

        //현재 카메라 크기를 파악한다.
        float MainCam_Aspect = MainCamera.aspect;
        Cam_Height = MainCamera.orthographicSize;
        Cam_Width = Cam_Height * MainCam_Aspect;

        //HQ가 있어야 할 위치에 HQ를 배치한다.
        GameObject P1_HQPivot = Instantiate(Resources.Load("Prefabs/1P_HQPivot", typeof(GameObject))) as GameObject;
        GameObject P2_HQPivot = Instantiate(Resources.Load("Prefabs/2P_HQPivot", typeof(GameObject))) as GameObject;
        P1_HQPivot.transform.position = new Vector3(-1f * Cam_Width, 0f, 0f);
        P2_HQPivot.transform.position = new Vector3(Cam_Width, 0f, 0f);

        //HQ를 저장한다.
        P1_HQ = P1_HQPivot.transform.Find("1P_HQ").gameObject;
        P2_HQ = P2_HQPivot.transform.Find("2P_HQ").gameObject;

        //(임시 코드) Unit을 저장한다.
        Set_CreatureList();

        //배경화면을 설정한다.
        BG = new GameObject[3];
        BG[0] = GameObject.Find("BackGround_G");
        BG[1] = GameObject.Find("BackGround_R");
        BG[2] = GameObject.Find("BackGround_B");
        Set_BG();
    }
    private void Set_BG()
    {
        int i = 0;
        char[] type = { 'G','R','B' };
        //순서대로 RGB
        foreach(char t in type)
        {

            if (0 <= Cur_Env[i] && Cur_Env[i] <= Env_L)
            {
                BG[i].GetComponent<Image>().sprite = Resources.Load("BG_Image/BackGround_" + t + "_L", typeof(Sprite)) as Sprite;
            }
            else if (Env_L < Cur_Env[i] && Cur_Env[i] <= Env_M)
            {
                BG[i].GetComponent<Image>().sprite = Resources.Load("BG_Image/BackGround_" + t + "_M", typeof(Sprite)) as Sprite;
            }
            else
            {
                BG[i].GetComponent<Image>().sprite = Resources.Load("BG_Image/BackGround_" + t + "_H", typeof(Sprite)) as Sprite;
            }
            i++;
        }
    }
    //각 플레이어가 선택한 크리쳐를 받아오는 함수, 
    //프로토타입에서는 정해진 Prefab에서 받아오지만 정식 버전에서는 다른 Scene에서 저장한 데이터를 받아오는 함수로 변경할 것.
    public void Set_CreatureList()
    {
   
        P1_CreatureList.Add(Resources.Load("Prefabs/Unit_D") as GameObject);
        P1_CreatureList.Add(Resources.Load("Prefabs/Unit_E") as GameObject);
        P2_CreatureList.Add(Resources.Load("Prefabs/Unit_D") as GameObject);
        P2_CreatureList.Add(Resources.Load("Prefabs/Unit_E") as GameObject);
    }

	// Update is called once per frame
	void Update () {

        RGBImage.color = new Vector4(Cur_Env[0] / Max_Env[0], Cur_Env[1] / Max_Env[1], Cur_Env[2] / Max_Env[2], 1);
        RGBText.text = "R: " + Cur_Env[0] + " G: " + Cur_Env[1] + " B: " + Cur_Env[2];

        //Key가 눌렸는지 테스트하는 항목
        //Key를 꾹 누르고 있다고 여러번 소환되지 않으며, 키가 눌리는 순간에 단 한번 인식한다.
        if (Input.anyKeyDown)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                //일시정지
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                P1_HQ.GetComponent<HeadQuarter>().Summon_Order(0);
                //Player 1 크리쳐 1 소환
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                P1_HQ.GetComponent<HeadQuarter>().Summon_Order(1);
                //Player 1 크리쳐 2 소환
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Player 1 크리쳐 3 소환
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                P2_HQ.GetComponent<HeadQuarter>().Summon_Order(0);
                //Player 2 크리쳐 1 소환
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                P2_HQ.GetComponent<HeadQuarter>().Summon_Order(1);
                //Player 2 크리쳐 2 소환
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                //Player 2 크리쳐 3 소환
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)||Input.GetKeyDown(KeyCode.Keypad1))
            {
                P1_HQ.GetComponent<SpellManager>().Spell_Load(false, 3);
                //Player 1 주문 1 시전
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                P1_HQ.GetComponent<SpellManager>().Spell_Load(true, 4);
                //Player 1 주문 2 시전
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                //Player 1 주문 3 시전
            }
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
            {
                P2_HQ.GetComponent<SpellManager>().Spell_Load(false, 3);
                //Player 2 주문 1 시전
            }
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                P2_HQ.GetComponent<SpellManager>().Spell_Load(true, 4);
                //Player 2 주문 2 시전
            }
            if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
            {
                //Player 2 주문 3 시전
            }
        }

    }

    public Vector3 Get_RGBValue()
    {
        return Cur_Env;
    }

    public void Destroyed(int Team)
    {
        //Team 승리! 
        //모든 Unit의 Timescale을 0으로 변경
        //혹은 Unit 고유의 승리 / 패배 애니메이션을 재생 
        //기획서에서 확정하면 그때 작성
    }

    public GameObject[] Get_UnitLIst(int Team, int MAX_UNIT_COUNT)
    {
        GameObject[] temp = new GameObject[MAX_UNIT_COUNT];
        try
        {
            for (int i = 0; i < MAX_UNIT_COUNT; i++)
            {
                if (Team == 1)
                {
                    GameObject p = Instantiate(P1_CreatureList[i]);
                    p.GetComponent<Unit>().ID = i;
                    p.SetActive(false);
                    temp[i] = p;
                }
                else
                {
                    GameObject p = Instantiate(P2_CreatureList[i]);
                    p.GetComponent<Unit>().ID = i;
                    p.SetActive(false);
                    temp[i] = p;
                }
            }
            return temp;
        }
        catch
        {
            return temp;
        }
    }


    public void Set_RGBValue(Vector3 RGB)
    {
        //미리 설정된 최대값과 최솟값을 넘지 않도록 값을 설정한다.
        Vector3 Limit_RGB = Limit_RGBValue(RGB);
        //Vector3형은 x,y,z를 하나씩 바꿀 수 없기 때문에 새로 만든 RGB 값을 할당해준다.
        Cur_Env = Limit_RGB;
        Set_BG();
    }
    public void Change_RGBValue(Vector3 dRGB)
    {
        Vector3 Res = Cur_Env + dRGB;
        Cur_Env = Limit_RGBValue(Res);
        Set_BG();
    }
    private Vector3 Limit_RGBValue(Vector3 RGB)
    {
        float R = Mathf.Min(Mathf.Max(Min_Env.x, RGB.x), Max_Env.x);
        float G = Mathf.Min(Mathf.Max(Min_Env.y, RGB.y), Max_Env.y);
        float B = Mathf.Min(Mathf.Max(Min_Env.z, RGB.z), Max_Env.z);
        return new Vector3(R, G, B);

    }


}
