using System;
using System.Collections.Generic;
using UnityEngine;

public class MinesweeperManager
{

    public Define.GameState gameState;

    //  public Action neighborZeroCheck; // 자기 주변 0칸들 누르기
    public Action GameOverAction; // 게임끝날때 호출
    public Action PressAction; // 클릭했을때 호출
    public Action DisclosureAction; // 겜끝나고 정체공개

    public List<Brick> bricks = new();

    public int leftBomb = 0;
    public int time = 0;

    public int horizontalCount;
    public int verticalCount;

    public bool isLeftPress = false;
    public bool isRigthPress = false;
    public bool isPressAnotherButton = false;


    public Sprite flagImg;
    public Sprite questionImg;
    public Sprite nullImg;

    public Sprite smaileImg1;
    public Sprite smaileImg2; 
    public Sprite smaileImg3; 
    public Sprite smaileImg4;

    public readonly int idle = Animator.StringToHash("Idle");
    public readonly int press = Animator.StringToHash("Press");
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

        list.RemoveAll(num => num >= horizontalCount * verticalCount || 0 > num);

    }
    public int NeighborBombCount(ref List<int> list)
    {
        int tmp = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (Main.Mine.bricks[list[i]]._isAmIBomb) tmp++;
        }
        return tmp;
    }
}
