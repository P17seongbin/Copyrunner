using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Text object에 이 스크립트를 적용

public class Information_display : MonoBehaviour
{
    public Text Information_Text;
    public Text WinText;

    private float Resource;
    private float HP;

    [SerializeField] private bool Is_Resource_Display; //UI가 자원을 표시할 때 이 값을 true로 설정하고 체력을 표시할 때 는 false로 설정한다. 
    [SerializeField] private bool Is_HP_Display;      //UI가 HP을 표시할 때 이 값을 true로 설정하고 자원을 표시할 때 는 false로 설정한다. 
    [SerializeField] private int player; //player 변수의 값에 따라 1P_HQ 또는 2P_HQ 의 Resource 또는 HP값을 참조한다. 사전에 1 또는 2의 값을 설정한다.

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Is_Resource_Display == true && Is_HP_Display == false)
            Resource_Update();
        else if (Is_Resource_Display == false && Is_HP_Display == true)
            Hp_Update();
    }

    private void Resource_Update()
    {
        if (player == 1)
            Resource = GameObject.Find("1P_HQ").GetComponent<HeadQuarter>().Get_Resource(); //HeadQuarter.cs 의 Get_Resource 함수에서 Resource 값을 받아온다.
        else if (player == 2)
            Resource = GameObject.Find("2P_HQ").GetComponent<HeadQuarter>().Get_Resource();

        Information_Text.text = Math.Round(Resource,2).ToString();
    }

    private void Hp_Update()
    {
        if (player == 1)
            HP = GameObject.Find("1P_HQ").GetComponent<HeadQuarter>().HQ_Health(); //HeadQuarter.cs 의 HQ_Health 함수에서 HP 값을 받아온다.
        else if (player == 2)
            HP = GameObject.Find("2P_HQ").GetComponent<HeadQuarter>().HQ_Health();

        Information_Text.text = "HP : " + HP.ToString();
    }

    public void Result_Display(int Team)
    {
        int TextNumber;

        if (Team == 1)
            TextNumber = 2;
        else
            TextNumber = 1;

        WinText.text = "Player " + TextNumber.ToString() + " win!";
    }

}
