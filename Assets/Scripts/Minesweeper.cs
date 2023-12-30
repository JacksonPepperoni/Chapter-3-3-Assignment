using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minesweeper : MonoBehaviour
{
    #region Field

    private bool _initialized = false;

    [SerializeField] private RectTransform _screenRect;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private TMP_Text _leftBombText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Button _smileBtn;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private GameObject _blocker;

    #endregion

    private void OnDisable()
    {
        CancelInvoke();

        Main.Mine.LoseAction -= GameLose;
        Main.Mine.WinAction -= GameWin;
        Main.Mine.ConditionCheckAction -= GameConditionCheck;
        _exitBtn.onClick.RemoveListener(GameExit);
        _smileBtn.onClick.RemoveListener(GameSetting);
    }

    public void Initialize()
    {
        if (_initialized) return;

        Main.Mine.flagImg = Main.Resource.Load<Sprite>($"Sprites/flag");
        Main.Mine.questionImg = Main.Resource.Load<Sprite>($"Sprites/question");
        Main.Mine.nullImg = Main.Resource.Load<Sprite>($"Sprites/none");
        Main.Mine.smaileImg1 = Main.Resource.Load<Sprite>($"Sprites/Smile1");
        Main.Mine.smaileImg2 = Main.Resource.Load<Sprite>($"Sprites/Smile2");
        Main.Mine.smaileImg3 = Main.Resource.Load<Sprite>($"Sprites/Smile3");
        Main.Mine.smaileImg4 = Main.Resource.Load<Sprite>($"Sprites/Smile4");

        _initialized = true;
    }

    public void GameSetting() // 난이도조절할것
    {
        Initialize();

        _blocker.SetActive(true);

        Main.Mine.ConditionCheckAction -= GameConditionCheck;
        Main.Mine.ConditionCheckAction += GameConditionCheck;
        Main.Mine.LoseAction -= GameLose;
        Main.Mine.LoseAction += GameLose;
        Main.Mine.WinAction -= GameWin;
        Main.Mine.WinAction += GameWin;

        _exitBtn.onClick.AddListener(GameExit);
        _smileBtn.onClick.AddListener(GameSetting);


        Main.Mine.LevelSetting();

        //TODO 난이도 설정에서 변경
        _screenRect.sizeDelta = Main.Mine.easyScreen;

        GameStart();
    }

    private void GameStart()
    {
        _smileBtn.interactable = false;
        _smileBtn.image.sprite = Main.Mine.smaileImg1;
        _gridLayoutGroup.constraintCount = Main.Mine.horizontalCount;

        BrickDistribute();
        BombDistribute();

        for (int i = 0; i < Main.Mine.bricks.Count; i++) // 이웃 최종정보가 필요해서 따로 실행
        {
            Main.Mine.bricks[i].Refresh();
        }

        _blocker.SetActive(false);
        Main.Mine.gameState = Define.GameState.Running;

        GameConditionCheck();
        TimerStart();
    }

    private void BrickDistribute() // 칸 생성
    {
        for (int i = 0; i < Main.Mine.bricks.Count; i++)
            Main.Pool._brickPool.Release(Main.Mine.bricks[i]);

        Main.Mine.bricks.Clear();

        for (int i = 0; i < Main.Mine.horizontalCount * Main.Mine.verticalCount; i++)
        {
            Brick brick = Main.Pool._brickPool.Get();
            brick.gameObject.name = $"{i}";
            brick.gameObject.transform.SetParent(_gridLayoutGroup.transform);
            brick.gameObject.transform.localScale = Vector3.one;
            brick.isAmIBomb = false;
            brick._id = i;

            Main.Mine.bricks.Add(brick);
        }

    }
    private void BombDistribute() // 폭탄 배부
    {
        int tmp = Main.Mine.bombCount;

        while (true)
        {
            int i = Random.Range(0, Main.Mine.bricks.Count);

            if (!Main.Mine.bricks[i].isAmIBomb)
            {
                Main.Mine.bricks[i].isAmIBomb = true;
                tmp--;
            }

            if (tmp <= 0) break;
        }
    }



    private void TimerStart()
    {
        CancelInvoke();

        if (_timerText == null)
            return;

        Main.Mine.time = 0;
        _timerText.text = $"{Main.Mine.time}";

        InvokeRepeating("TimeIsRunningOut", 1, 1); // 오브젝트 비활성화 상태에서도 작동하는 invoke 쓰기
    }

    private void TimeIsRunningOut()
    {
        Main.Mine.time++;
        _timerText.text = $"{Mathf.Clamp(Main.Mine.time, 0, 999)}";
    }

    private void TimerStop()
    {
        CancelInvoke();
    }

    public void GameExit()
    {
        this.gameObject.SetActive(false);
    }


    private void GameConditionCheck() // 게임끝내기 체크
    {
        _leftBombText.text = $"{Mathf.Clamp(Main.Mine.fakeBombCount, -99, 999)}";

        if (Main.Mine.isDead)
        {
            Main.Mine.LoseAction?.Invoke();
            return;
        }


        if (Main.Mine.aliveBicksCount == Main.Mine.bombCount)
        {
            Main.Mine.WinAction?.Invoke();
        }

    }

    private void GameWin()
    {
        Main.Mine.gameState = Define.GameState.GameOver;
        _blocker.SetActive(true);
        TimerStop();

        _smileBtn.image.sprite = Main.Mine.smaileImg3;
        _leftBombText.text = $"{0}";
        _smileBtn.interactable = true;
    }

    private void GameLose()
    {
        Main.Mine.gameState = Define.GameState.GameOver;
        _blocker.SetActive(true);
        TimerStop();

        _smileBtn.image.sprite = Main.Mine.smaileImg2;
        _leftBombText.text = $"{Main.Mine.currentBombCount}";
        _smileBtn.interactable = true;
    }
}
