using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpell_A : MonoBehaviour {

    public float Speed; //마커가 움직일 때의 속도
    public int Team;

    private bool Is_Shooting;

	// Use this for initialization
	void Start () {

        Is_Shooting = false;

	}
	
	// Update is called once per frame
	void Update () {

        //Key가 눌렸는지 테스트하는 항목
        //Key를 꾹 누르고 있으면 계속 인식한다.
        if (Team == 1) //Team이 1이라면
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Is_Shooting = true;
                //레이저를 1초간 쏜다.
            }
            else if (!Is_Shooting)
            {
                transform.position += new Vector3(Input.GetAxis("Horizontal1"), 0, 0) * Time.deltaTime;
            }
        }
        else //Team이 -1이라면
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Is_Shooting = true;
                //레이저를 1초간 쏜다.
            }
            else if (!Is_Shooting)
            {
                transform.position += new Vector3(Input.GetAxis("Horizontal2"), 0, 0) * Time.deltaTime;
            }
        }
    }

    void Laser_Beam()
    {

    }
}
