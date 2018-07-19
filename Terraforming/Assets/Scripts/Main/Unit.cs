﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Max_Health, Health;// 최대 체력수치/현재 체력수치를 나타냅니다, Unity 편집기에서 각 Unit별로 고유의 값을 지정합니다.
    public float Cost; // 이 Unit을 소환하는데 필요한 자원(에너지) 의 양입니다.Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.
    public float Speed; //각 Unit의 고유 속도입니다, 단위는 Unity Meter입니다.Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.
    public float Summon_Time; //각 Unit의 고유 소환시간입니다. 단위는 초입니다. Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.
    public float Attack_Speed; //각 Unit이 공격을 1회 하는데 걸리는 시간을 의미합니다.Unity 편집기에서 각 Unit별로 별도의 값을 지정한 뒤 Prefab화 합니다.

    private Vector3 Cur_Env; //매 Frame마다 GameManager에게 현재 환경 변수를 받아오는 것을 저장할 임시 변수입니다.
    private bool Is_Moveable; //현재 Unit.cs가 붙어 있는 Object가 움직일 수 있는지를 나타냅니다. 기본값은 True이며, 앞에 공격할 수 있는 다른 Object가 있거나 health가 0보다 작아지면 False로 변경됩니다.
    private GameObject HQ; //Unit을 소환한 HeadQuarter GameObject를 저장합니다.
    private int ID; //이 Unit의 번호를 나타냅니다. (할당된 슬롯 순서)
    // Use this for initialization
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(int Team, int _ID, GameObject HeadQuarter)
    {

    }
    public void Hit(float damage)
    {

    }
    public float Get_Cost()
    {

    }
    public float Get_SummonTime()
    {

    }
    private void Move()
    {

    }
    private void Check_Dead()
    {

    }
    private void Get_RGBValue()
    {

    }
    IEnumerator Dead_Anim()
    {

    }
    
}
