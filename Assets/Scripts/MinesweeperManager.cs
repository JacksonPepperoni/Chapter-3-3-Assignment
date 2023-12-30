using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static Define;

public class MinesweeperManager
{

    #region 난이도

    public readonly int[] easy = new int[] { 9, 9, 10 };
    public readonly int[] normal = new int[] { 16, 16, 40 };
    public readonly int[] hard = new int[] { 30, 16, 99 };

    public readonly Vector2 easyScreen = new Vector2(172f, 253f);
    public readonly Vector2 normalScreen = new Vector2(284f, 365f);
    public readonly Vector2 hardScreen = new Vector2(508f, 365f);

    /*
     * json으로 바꿀것
         초급 : 가로 9, 세로 9, 지뢰 10개 (12.3%)  172   253
       중급 : 가로 16, 세로 16, 지뢰 40개 (15.6%)    284  365
        고급 : 가로 30, 세로 16, 지뢰 99개 (20.6%)     = 판 가로 길이 508 x 365
     */

    #endregion

    #region Field

    public List<Brick> bricks = new();

    public Define.GameState gameState = Define.GameState.GameOver;
    public Define.GameLevel gamelevel = Define.GameLevel.Easy;

    public Action ConditionCheckAction; // 게임 ui 새로고침 + 게임승패 판단
    public Action LoseAction;
    public Action WinAction;

    public int horizontalCount;
    public int verticalCount;
    public int bombCount = 0;
    public int currentBombCount = 0;
    public int fakeBombCount = 0;
    public int aliveBicksCount = 0;
    public int time = 0;

    public bool isDead = false;
    public bool isLeftPress = false;
    public bool isRigthPress = false;
    public bool isPressAnotherButton = false;
    public bool isShield = false;

    public Sprite flagImg;
    public Sprite questionImg;
    public Sprite nullImg;
    public Sprite smaileImg1;
    public Sprite smaileImg2;
    public Sprite smaileImg3;
    public Sprite smaileImg4;
    public Sprite normalCap;
    public Sprite pressCap;


   // public readonly int idle = Animator.StringToHash("Idle");
   // public readonly int press = Animator.StringToHash("Press");
    public readonly int number = Animator.StringToHash("Number");
    public readonly int clickBomb = Animator.StringToHash("ClickBomb");
    public readonly int bomb = Animator.StringToHash("Bomb");
    public readonly int notBomb = Animator.StringToHash("NotBomb");

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

    public void LevelSetting()
    {
        switch (gamelevel)
        {
            default:
            case GameLevel.Easy:
                EasySet();
                break;
            case GameLevel.Normal:
                NormalSet();
                break;
            case GameLevel.Hard:
                HardSet();
                break;
        }

        currentBombCount = bombCount;
        fakeBombCount = bombCount;
        aliveBicksCount = horizontalCount * verticalCount;

        isLeftPress = false;
        isRigthPress = false;
        isPressAnotherButton = false;
        isShield = false;
        isDead = false;
    }

    public void BrickNeighborCheck(ref List<int> list, int id)
    {
        list.Clear();
        list.Add(id + 1);
        list.Add(id - 1);

        list.Add(id + horizontalCount);
        list.Add(id + horizontalCount + 1);
        list.Add(id + horizontalCount - 1);

        list.Add(id - horizontalCount);
        list.Add(id - horizontalCount + 1);
        list.Add(id - horizontalCount - 1);

        if ((id + 1) % horizontalCount == 0)
        {
            for (int i = 0; i <= verticalCount; i++)
                list.Remove(horizontalCount * i);
        }

        if (id % horizontalCount == 0)
        {
            for (int i = 0; i <= verticalCount; i++)
                list.Remove(horizontalCount * i - 1);
        }

        list.RemoveAll(num => num >= horizontalCount * verticalCount || 0 > num);

    }

    public int NeighborBombCount(ref List<int> list)
    {
        int count = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (Main.Mine.bricks[list[i]].isAmIBomb)
                count++;
        }

        return count;
    }

    public void ZeroInfection(int id)
    {
        for (int i = 0; i < bricks[id].neighborNums.Count; i++)
        {
            bricks[bricks[id].neighborNums[i]].Pressed();
        }
    }
    public bool IsGameOver()
    {
        return gameState == Define.GameState.GameOver;
    }

    private void EasySet()
    {
        horizontalCount = easy[0];
        verticalCount = easy[1];
        bombCount = easy[2];
    }

    private void NormalSet()
    {
        horizontalCount = normal[0];
        verticalCount = normal[1];
        bombCount = normal[2];
    }
    private void HardSet()
    {
        horizontalCount = hard[0];
        verticalCount = hard[1];
        bombCount = hard[2];
    }

}
