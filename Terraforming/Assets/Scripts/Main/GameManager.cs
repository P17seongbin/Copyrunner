using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Vector3 Max_Env, Min_Env;
    private Vector3 Cur_Env;
    private float Init_R, Init_G, Init_B;

    // Use this for initialization
    void Start () {
        //초기값을 설정하고 제한값을 넘지 않도록 조율해줍니다.
        Init_R = 5f;
        Init_G = 5f;
        Init_B = 5f;
        Cur_Env = new Vector3(Init_R, Init_G, Init_B);
        Cur_Env = Limit_RGBValue(Cur_Env);
	}

	// Update is called once per frame
	void Update () {


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
                //Player 1 크리쳐 1 소환
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                //Player 1 크리쳐 2 소환
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Player 1 크리쳐 3 소환
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                //Player 2 크리쳐 1 소환
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                //Player 2 크리쳐 2 소환
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                //Player 2 크리쳐 3 소환
            }


            if (Input.GetKeyDown(KeyCode.Alpha1)||Input.GetKeyDown(KeyCode.Keypad1))
            {
                //Player 1 주문 1 시전
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                //Player 1 주문 2 시전
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                //Player 1 주문 3 시전
            }
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
            {
                //Player 2 주문 1 시전
            }
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            {
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


    public void Set_RGBValue(Vector3 RGB)
    {
        //미리 설정된 최대값과 최솟값을 넘지 않도록 값을 설정한다.
        Vector3 Limit_RGB = Limit_RGBValue(RGB);
        //Vector3형은 x,y,z를 하나씩 바꿀 수 없기 때문에 새로 만든 RGB 값을 할당해준다.
        Cur_Env = Limit_RGB;
    }
    public void Change_RGBValue(Vector3 dRGB)
    {
        Vector3 Res = Cur_Env + dRGB;
        Cur_Env = Limit_RGBValue(Res);
    }
    private Vector3 Limit_RGBValue(Vector3 RGB)
    {
        float R = Mathf.Min(Mathf.Max(Min_Env.x, RGB.x), Max_Env.x);
        float G = Mathf.Min(Mathf.Max(Min_Env.y, RGB.y), Max_Env.y);
        float B = Mathf.Min(Mathf.Max(Min_Env.z, RGB.z), Max_Env.z);
        return new Vector3(R, G, B);
    }
}
