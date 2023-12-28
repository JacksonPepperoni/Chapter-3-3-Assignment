using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinesweeperManager
{
    //public 인데 _붙은 변수명 전부 수정할것

    public Define.GameState _gameState;

    //  public Action neighborZeroCheck; // 자기 주변 0칸들 누르기
    public Action gameOver; // 게임끝날때 호출

    public Action PressAction; // 클릭했을때 호출


    public List<Brick> _bricks = new();

    public int _leftBomb = 0;
    public int _time = 0;

    public int _horizontalCount;
    public int _verticalCount;

    public bool _isLeftPress = false;
    public bool _isRigthPress = false;
    public bool _isPressAnotherButton = false;

    public Sprite _flagImg;
    public Sprite _questionImg;
    public Sprite _nullImg;

    public readonly int _idle = Animator.StringToHash("Idle");
    public readonly int _press = Animator.StringToHash("Press");
    public readonly int _number = Animator.StringToHash("Number");
    public readonly int _clickBomb = Animator.StringToHash("ClickBomb");
    public readonly int _bomb = Animator.StringToHash("Bomb");
    public readonly int _notBomb = Animator.StringToHash("NotBomb");

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

    public void BrickNeighborCheck(ref List<int> list, int id)
    {
        list.Clear();
        list.Add(id + 1);
        list.Add(id - 1);

        list.Add(id + _horizontalCount);
        list.Add(id + _horizontalCount + 1);
        list.Add(id + _horizontalCount - 1);

        list.Add(id - _horizontalCount);
        list.Add(id - _horizontalCount + 1);
        list.Add(id - _horizontalCount - 1);

        list.RemoveAll(num => num >= _horizontalCount * _verticalCount || 0 > num);

    }
    public int NeighborBombCount(ref List<int> list)
    {
        int tmp = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (Main.Mine._bricks[list[i]]._isAmIBomb)
                tmp++;
        }
        return tmp;
    }
}
