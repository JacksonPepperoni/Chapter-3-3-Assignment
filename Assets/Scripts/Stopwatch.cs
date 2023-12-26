using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Stopwatch : MonoBehaviour
{


    /// <summary>
    /// ������ ������ ���ǽð� �����ϰ� ����� �귶���� â ���������� Ȯ���ϱ�.   
    /// �� �׳� ������Ʈ ��Ȱ��ȭ ���¿����� �۵��ϴ� invoke������ ���°� ������ 
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
        Debug.Log("�ð��帥��");
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

        Debug.Log("�ð������");
    }
}
