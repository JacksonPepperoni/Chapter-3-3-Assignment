using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{

    private int _leftBombCount;
    private TMP_Text _leftBombCountText;


    public Action Refresh; // ��ư Ŭ���Ҷ����� �����

    public List<bool> bombsMap; //��ź�̸� true �ƴϸ� false


    /// <summary>
    /// ��߼���ŭ ���� ��ź ���� ��������
    /// 
    /// ��� �� �ȼ����� ������ ��ź�ۿ� ������ Ŭ����Ȱŷ� ������
    /// 
    /// </summary>


    #region ��ȣ ����
    public readonly Dictionary<int, Color> numberColors = new()
    {
            { 1, Color.blue },
            { 2, new Color(0, 128f, 0) },
            { 3, Color.red },
            { 4, new Color(0, 0, 128f) },
            { 5, new Color(128f, 0, 0) },
            { 6, new Color(0, 128f, 128f) },
            { 7, Color.black },
            { 8, new Color(128f, 128f, 128f) }
     };
    #endregion


    private void Awake()
    {
       
    }


    void Initialize()
    {
        // �ʰ�����ŭ ��ư ��ȯ
        // �� ������ŭ ���� ȭ�� ����


        _leftBombCountText.text = $"{_leftBombCount}";

    }

}
