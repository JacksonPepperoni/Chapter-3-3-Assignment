using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Stopwatch : MonoBehaviour
{


    /// <summary>
    /// 게임을 시작한 현실시간 저장하고 몇분이 흘렀는지 창 켜질때마다 확인하기.   
    /// 아 그냥 오브젝트 비활성화 상태에서도 작동하는 invoke같은거 쓰는게 나을듯 
    /// </summary>
    /// 
    private TMP_Text _timerText;
    private int _count = 0;

    private void Awake()
    {
        _timerText = GetComponentInChildren<TMP_Text>();
    }

    public void TimerStart()
    {
        Debug.Log("시간흐른다");
        CancelInvoke();

        _count = 0;
        _timerText.text = $"{_count}";
        InvokeRepeating("MeasureTheTime", 1, 1);
    }

    private void MeasureTheTime()
    {
        _count++;
        _timerText.text = $"{_count}";
    }
    public void TimerStop()
    {
        CancelInvoke();

        Debug.Log("시간멈춘다");
    }
}
