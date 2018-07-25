using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SummonManager : MonoBehaviour {

    public int Team;
    int MAX_QUEUE_SIZE = 6;
    int CUR_QUEUE_SIZE = 0;

    List<GameObject> Unit_Queue;

    private GameObject SummonQueue_UI;
    // Use this for initialization
    private float XSize;
    private bool Is_Summoning;
    private float Summon_StartTime;
    private float Summon_CastTime;

  [SerializeField]  private Sprite Null_Queue;

    void Start () {
        if (Team == 1)
        {
            SummonQueue_UI = GameObject.Find("P1_SummonQueue");
        }
        else
            SummonQueue_UI = GameObject.Find("P2_SummonQueue");
        MAX_QUEUE_SIZE = 6;
        CUR_QUEUE_SIZE = 0;
        XSize = GetComponent<BoxCollider2D>().size.x;

        Unit_Queue = new List<GameObject>();
        for (int i = 0; i < MAX_QUEUE_SIZE; i++)
            Unit_Queue.Add(null);
        Is_Summoning = false;
        Queue_Image_Update();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Unit_Queue[0] != null && !Is_Summoning)
        {
            Is_Summoning = true;
            Summon_StartTime = Time.fixedTime;
            Summon_CastTime = Unit_Queue[0].GetComponent<Unit>().Get_SummonTime();
        }


        if(Is_Summoning && Time.fixedTime - Summon_StartTime >= Summon_CastTime)
        {
            Is_Summoning = false;
            Summon_Unit(Unit_Queue[0]);
            Unit_Queue[0] = null;
            for(int i=1;i<MAX_QUEUE_SIZE;i++)
            {
                Unit_Queue[i - 1] = Unit_Queue[i];
            }
            CUR_QUEUE_SIZE--;
            Queue_Image_Update();
        }
	}
    public GameObject Enqueue_Unit(GameObject Unit)
    {
        GameObject temp = Instantiate(Unit);
        if (!Is_Full())
        {
            Unit_Queue[CUR_QUEUE_SIZE] = temp;
            CUR_QUEUE_SIZE++;
            Queue_Image_Update();
            return temp;

            
        }
        else
            return null;
    }
    private void Queue_Image_Update()
    {
        for(int i=0;i<MAX_QUEUE_SIZE;i++)
        {
            if (i < CUR_QUEUE_SIZE)
                SummonQueue_UI.transform.Find("Queue_" + i.ToString()).GetComponent<Image>().sprite = Unit_Queue[i].GetComponent<SpriteRenderer>().sprite;
            else
                SummonQueue_UI.transform.Find("Queue_" + i.ToString()).GetComponent<Image>().sprite = Null_Queue;
        }
    }
    private GameObject Summon_Unit(GameObject Unit)
    {

        GameObject res = Unit;
        res.SetActive(true);
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
        return CUR_QUEUE_SIZE >= MAX_QUEUE_SIZE;
    }
}
