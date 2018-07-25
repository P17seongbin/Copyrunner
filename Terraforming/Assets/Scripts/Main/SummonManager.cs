using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SummonManager : MonoBehaviour {

    public int Team;
    int MAX_QUEUE_SIZE = 6;
    int CUR_QUEUE_SIZE = 0;

    List<GameObject> Unit_Queue;
	// Use this for initialization
	void Start () {
        Unit_Queue = new List<GameObject>(MAX_QUEUE_SIZE);

	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}
    public GameObject Enqueue_Unit(GameObject Unit)
    {
        GameObject temp = Instantiate(Unit);
        if (!Is_Full())
        {
            Unit_Queue[CUR_QUEUE_SIZE] = temp;
            CUR_QUEUE_SIZE++;
            return temp;
        }
        else
            return null;
    }

    private GameObject Summon_Unit(GameObject Unit)
    {

            GameObject res = Instantiate(Unit);
            //res의 위치를 HQ의 위치를 통해 계산한다.
            //지금은 기본 위치에 소환하지만 필요하다면 유닛별로 다른 위치를 할당하는 기능을 추가할 것.
            float Unit_XSize = res.GetComponent<BoxCollider2D>().size.x;
            res.transform.position = new Vector3((transform.position.x + (Team * (XSize + Unit_XSize) / 2f)), 0f, 0f);
            res.transform.rotation = Quaternion.Euler(new Vector3(0f, (90f - Team * 90f), 0f));
            res.GetComponent<Unit>().Init(Team, gameObject);


            return res;

    }



    public bool Is_Full()
    {
        return Unit_Queue.Count >= MAX_QUEUE_SIZE;
    }
}
