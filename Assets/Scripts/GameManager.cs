using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Field

    public Define.GameState _gameState;

    private int _leftBombCount = 0;
    private TMP_Text _leftBombCountText;


    public Action neighborZeroCheck; // 자기 주변 0칸들 누르기
    public Action aroundBrickView; // 왼쪽클릭중에 오른클릭눌러서 범위볼때
    public Action gameOver; // 게임끝날때 호출

    private List<bool> _bombsMap; //폭탄이면 true 아니면 false

    private int _horizontalLength = 9;

    public Transform brickPanel;

    private Sprite _flagImg;
    private Sprite _questionImg;
    private Sprite _nullImg;


    #endregion

    #region 숫자색깔

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


    private void Start()
    {
        _flagImg = Main.Resource.Load<Sprite>($"Sprites/flag");
        _questionImg = Main.Resource.Load<Sprite>($"Sprites/question");
        _nullImg = Main.Resource.Load<Sprite>($"Sprites/none");
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < 54; i++)
        {
            Brick brick = Main.Resource.Instantiate("Brick", null, true).GetComponent<Brick>();
            brick.transform.SetParent(brickPanel);
            brick.id = i;
            brick.neighborBombCount = NumberOfAmbientBombs(i);

            brick._flagImg = _flagImg;
            brick._questionImg = _questionImg;
            brick._nullImg = _nullImg;

            brick.Initialize();
        }

        _gameState = Define.GameState.Running;
    }

    public void GameConditionUpdate()
    {
        _gameState = Define.GameState.GameOver;

        // 폭탄 개수, 남은 시간등등 표시

    }

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

    bool BombsCountLimitCheck(int id)
    {
        if (id < _bombsMap.Count && 0 <= id)
            return true;
        else
            return false;
    }

}
