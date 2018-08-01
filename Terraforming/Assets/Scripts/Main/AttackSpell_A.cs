using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpell_A : MonoBehaviour {

    public float Speed; //마커가 움직일 때의 속도
    public int Team;

    private bool Is_Shooting;
    List<GameObject> Ranged_Target = new List<GameObject>();//타격 범위 안에 들어온 오브젝트 리스트.

	// Use this for initialization
	void Start () {

        Is_Shooting = false;

	}
	
	// Update is called once per frame
	void Update () {

        //Key가 눌렸는지 테스트하는 항목
        if (Team == 1) //Team이 1이라면
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Laser_Beam();
                //레이저를 1초간 쏜다.
            }
            else if (!Is_Shooting)
            {
                transform.position += new Vector3(Input.GetAxis("Horizontal1"), 0, 0) * Time.deltaTime * Speed;
            }
        }
        else //Team이 -1이라면
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Laser_Beam();
                //레이저를 1초간 쏜다.
            }
            else if (!Is_Shooting)
            {
                transform.position += new Vector3(Input.GetAxis("Horizontal2"), 0, 0) * Time.deltaTime * Speed;
            }
        }
    }

    void Laser_Beam()
    {
        if (!Is_Shooting)
        {
            Is_Shooting = true;
            if (Ranged_Target.Count > 0)
                //범위 안에 있는 모든 Unit에게 데미지를 준다.
                foreach (GameObject t in Ranged_Target)
                {
                    if (t.tag == "Unit")
                        t.GetComponent<Unit>().Pierce_Hit(5f);
                }
            //레이저를 발사하는 이펙트를 만들어야 한다 여기서

            Invoke("End_Beam", 1.0f);
        }
    }
    void End_Beam()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Is_Shooting)
        {
            if (!Ranged_Target.Contains(collision.gameObject))
            {
                Ranged_Target.Add(collision.gameObject);
            }
        }
        else
        {

            if (collision.gameObject.tag == "Unit")
                collision.gameObject.GetComponent<Unit>().Pierce_Hit(5f);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Ranged_Target.Contains(collision.gameObject))
        {
            Ranged_Target.Remove(collision.gameObject);
        }
    }
}
