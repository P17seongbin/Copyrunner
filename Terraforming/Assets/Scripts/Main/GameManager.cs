using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Vector3 Max_Env, Min_Env;
    private Vector3 Cur_Env;
    private float Init_R, Init_G, Init_B;
    public Vector3 Get_RGBValue()
    {
        return Cur_Env;
    }
    public void Set_RGBValue(Vector3 RGB)
    {
        //미리 설정된 최대값과 최솟값을 넘지 않도록 값을 설정한다.
        Vector3 Limit_RGB = Limit_RGBValue(RGB);
        //Vector3형은 x,y,z를 하나씩 바꿀 수 없기 때문에 새로 만든 RGB 값을 할당해준다.
        Cur_Env = Limit_RGB;
    }
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
		
	}
    private Vector3 Limit_RGBValue(Vector3 RGB)
    {
        float R = Mathf.Min(Mathf.Max(Min_Env.x, RGB.x), Max_Env.x);
        float G = Mathf.Min(Mathf.Max(Min_Env.y, RGB.y), Max_Env.y);
        float B = Mathf.Min(Mathf.Max(Min_Env.z, RGB.z), Max_Env.z);
        return new Vector3(R, G, B);
    }
}
