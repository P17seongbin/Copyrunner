using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Text object에 이 스크립트를 적용

public class Resouce_display : MonoBehaviour
{
    private float Resource;
    public Text Resource_Text;

    [SerializeField] private int player; //player 변수의 값에 따라 1P_HQ 또는 2P_HQ 의 Resource값을 참조한다. 사전에 1 또는 2의 값을 설정한다.

    // Use this for initialization
    void Start ()
    {


    }
	
	// Update is called once per frame
	void Update ()
    {
        if(player == 1)
             Resource = GameObject.Find("1P_HQ").GetComponent<HeadQuarter>().Get_Resource(); //HeadQuarter.cs 의 Get_Resource 함수에서 Resource 값을 받아온다.
        else if(player == 2)
             Resource = GameObject.Find("2P_HQ").GetComponent<HeadQuarter>().Get_Resource();

        Resource_Text.text = "Resource : " + Resource.ToString();

    }
}
