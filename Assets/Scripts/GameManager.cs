using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    private int _leftBombCount = 0;
    private TMP_Text _leftBombCountText;


    public Action Refresh; // 버튼 클릭할때마다 실행됨

    private List<bool> _bombsMap; //폭탄이면 true 아니면 false


    private int _horizontalLength = 9;


    public Transform brickPanel;

    /// <summary>
    /// 깃발수만큼 남은 폭탄 갯수 빠져야함
    /// 
    /// 깃발 다 안세워도 남은게 폭탄밖에 없으면 클리어된거로 떠야함
    /// 
    /// </summary>


    #region 숫자 색깔

    public readonly Dictionary<int, Color> numberColors = new()
    {
            { 0, Color.clear },
            { 1, Color.blue },
            { 2, new Color(0, 128f / 255f, 0) },
            { 3, Color.red },
            { 4, new Color(0, 0, 128f / 255f) },
            { 5, new Color(128f / 255f, 0, 0) },
            { 6, new Color(0, 128f / 255f, 128f / 255f) },
            { 7, Color.black },
            { 8, new Color(128f / 255f, 128f / 255f, 128f / 255f) }
     };

    #endregion



    public int NumberOfAmbientBombs(int id)
    {
        int num = 0;

        /*

        num += _bombsMap[id] ? 1 : 0;
        num += _bombsMap[id + 1] ? 1 : 0;
        num += _bombsMap[id - 1] ? 1 : 0;

        num += _bombsMap[id + _horizontalLength] ? 1 : 0;
        num += _bombsMap[id + _horizontalLength + 1] ? 1 : 0;
        num += _bombsMap[id + _horizontalLength - 1] ? 1 : 0;

        num += _bombsMap[id - _horizontalLength] ? 1 : 0;
        num += _bombsMap[id - _horizontalLength + 1] ? 1 : 0;
        num += _bombsMap[id - _horizontalLength - 1] ? 1 : 0;
                */

        num = UnityEngine.Random.Range(0, numberColors.Count);
        


        return num;
    }

    bool BombsCountLimitCheck(int i)
    {
        if (i < _bombsMap.Count && 0 <= i)
            return true;
        else
            return false;
    }


    private void Start()
    {
        Initialize();
    }


    void Initialize()
    {
        for (int i = 0; i < 54; i++)
        {
            Brick brick = Main.Resource.Instantiate("Brick", null, true).GetComponent<Brick>();
            brick.transform.SetParent(brickPanel);
            brick.id = i;
            brick.Initialize();

        }

        // 맵갯수만큼 버튼 소환
        // 그 갯수만큼 게임 화면 조정


        //  _leftBombCountText.text = $"{_leftBombCount}";

    }

}
