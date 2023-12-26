using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{

    private int _leftBombCount;
    private TMP_Text _leftBombCountText;


    public Action Refresh; // 버튼 클릭할때마다 실행됨

    public List<bool> bombsMap; //폭탄이면 true 아니면 false


    /// <summary>
    /// 깃발수만큼 남은 폭탄 갯수 빠져야함
    /// 
    /// 깃발 다 안세워도 남은게 폭탄밖에 없으면 클리어된거로 떠야함
    /// 
    /// </summary>


    #region 번호 색깔
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
        // 맵갯수만큼 버튼 소환
        // 그 갯수만큼 게임 화면 조정


        _leftBombCountText.text = $"{_leftBombCount}";

    }

}
