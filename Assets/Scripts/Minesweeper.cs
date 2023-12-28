using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Minesweeper : MonoBehaviour
{

    #region Field

    private bool _initialized = false;

    [SerializeField] private TMP_Text _leftBombText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private GameObject blocker;

    private List<bool> _bombsMap = new(); //폭탄이면 true 아니면 false

    #endregion


    private void OnEnable()
    {
        Initialize();
        GameSetting();
    }
    public void Initialize()
    {
        if (_initialized) return;

        blocker.SetActive(false);
        Main.Mine._flagImg = Main.Resource.Load<Sprite>($"Sprites/flag");
        Main.Mine._questionImg = Main.Resource.Load<Sprite>($"Sprites/question");
        Main.Mine._nullImg = Main.Resource.Load<Sprite>($"Sprites/none");

        Main.Mine.PressAction -= GameUiUpdate;
        Main.Mine.PressAction += GameUiUpdate;
        Main.Mine.gameOver -= GameEnd;
        Main.Mine.gameOver += GameEnd;
        Main.Mine.gameOver -= TimerStop;
        Main.Mine.gameOver += TimerStop;

        _initialized = true;
    }

    public void GameSetting()
    {

        Main.Mine._horizontalCount = 9; // 가로 칸 갯수
        Main.Mine._verticalCount = 6;
        Main.Mine._leftBomb = 0;

        GameUiUpdate();
        GameStart();
    }

    private void GameStart()
    {
        Main.Mine._isLeftPress = false;
        Main.Mine._isRigthPress = false;
        Main.Mine._isPressAnotherButton = false;

        gridLayoutGroup.constraintCount = Main.Mine._horizontalCount;

        for (int i = 0; i < Main.Mine._horizontalCount * Main.Mine._verticalCount; i++)
        {
            GameObject obj = Main.Resource.Instantiate("Brick", null, true);
            obj.name += i;
            Brick brick = obj.GetComponent<Brick>();
            brick.transform.SetParent(gridLayoutGroup.transform);
            brick.transform.localScale = Vector3.one;

            brick._isAmIBomb = Random.Range(0, 10) < 2;
            brick._id = i;

            Main.Mine._bricks.Add(brick);
        }


        for (int i = 0; i < Main.Mine._bricks.Count; i++)
        {
            Main.Mine._bricks[i].Initialize();

            if (Main.Mine._bricks[i]._isAmIBomb)
                Main.Mine._leftBomb++;
        }

        Main.Mine._gameState = Define.GameState.Running;
        
        GameUiUpdate();
        TimerStart();
    }

    private void GameEnd()
    {
        Main.Mine._gameState = Define.GameState.GameOver;
        blocker.SetActive(true);
    }

    private void GameUiUpdate()
    {
        if (_leftBombText == null)
            return;

        _leftBombText.text = $"{Mathf.Clamp(Main.Mine._leftBomb, -99, 999)}";
    }

    private void TimerStart()
    {
        CancelInvoke();

        if (_timerText == null)
            return;

        Main.Mine._time = 0;
        _timerText.text = $"{Main.Mine._time}";

        InvokeRepeating("TimeIsRunningOut", 1, 1); // 오브젝트 비활성화 상태에서도 작동하는 invoke 쓰기
    }

    private void TimeIsRunningOut()
    {
        Main.Mine._time++;
        _timerText.text = $"{Mathf.Clamp(Main.Mine._time, 0, 999)}";
    }

    private void TimerStop()
    {
        CancelInvoke();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        CancelInvoke();
    }
}
