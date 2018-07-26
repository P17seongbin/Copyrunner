using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    protected float FrameRate;//Unit의 Update()가 1초에 몇번 호출되는지를 나타냅니다. Unit_TimeScale의 값이 1일 때를 기준으로 합니다.
    protected float DeltaTime;
    protected float TimeScale;//고유의 TimeScale을 나타냅니다, 일시정지 및 감속 효과에 사용됩니다.



    protected bool Refresh_TimeCheck()
    {
        bool res = ((DeltaTime) * TimeScale) >= (1f / FrameRate);
        if (res)
            DeltaTime = 0f;
        return res;
    }
    public void Set_TimeScale(float value)
    {
        TimeScale = value;
    }

    public float Get_TimeScale()
    {
        return TimeScale;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
