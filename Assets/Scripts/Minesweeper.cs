using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minesweeper : MonoBehaviour
{

    #region Field

    private bool _initialized = false;

    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private TMP_Text _leftBombText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Button _smileBtn;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private GameObject _blocker;

    #endregion


    private void OnEnable()
    {
        //   Input.multiTouchEnabled = false;

        Initialize();
        GameSetting();
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
        _blocker.SetActive(true);

        Main.Mine.PressAction -= GameUiUpdate;
        Main.Mine.PressAction += GameUiUpdate;
        Main.Mine.GameOverAction -= GameEnd;
        Main.Mine.GameOverAction += GameEnd;
        _exitBtn.onClick.AddListener(GameExit);
        _smileBtn.onClick.AddListener(GameSetting);

        Main.Mine.horizontalCount = 9;
        Main.Mine.verticalCount = 6;
        Main.Mine.leftBomb = 0;

        GameStart();
    }

    private void GameStart()
    {
        _smileBtn.interactable = false;
        _smileBtn.image.sprite = Main.Mine.smaileImg1;

        Main.Mine.isLeftPress = false;
        Main.Mine.isRigthPress = false;
        Main.Mine.isPressAnotherButton = false;

        _gridLayoutGroup.constraintCount = Main.Mine.horizontalCount;

        Main.Mine.bricks.Clear();

        foreach (Transform child in _gridLayoutGroup.transform)
        {
            Main.Resource.Destroy(child.gameObject);
        }


        for (int i = 0; i < Main.Mine.horizontalCount * Main.Mine.verticalCount; i++)
        {
            GameObject obj = Main.Resource.Instantiate("Brick", null, true);
            obj.name += i;
            Brick brick = obj.GetComponent<Brick>();
            brick.transform.SetParent(_gridLayoutGroup.transform);
            brick.transform.localScale = Vector3.one;

            brick.isAmIBomb = Random.Range(0f, 10f) < 1.7f;
            brick._id = i;

            Main.Mine.bricks.Add(brick);
        }

        for (int i = 0; i < Main.Mine.bricks.Count; i++)
        {
            Main.Mine.bricks[i].Initialize();
            Main.Mine.bricks[i].Refresh();

            if (Main.Mine.bricks[i].isAmIBomb)
                Main.Mine.leftBomb++;
        }

        Main.Mine.gameState = Define.GameState.Running;

        _blocker.SetActive(false);

        GameUiUpdate();
        TimerStart();
    }

    private void GameEnd()
    {
        Main.Mine.gameState = Define.GameState.GameOver;
        _blocker.SetActive(true);

        Main.Mine.DisclosureAction?.Invoke();

        if (Main.Mine.leftBomb == 0)
            _smileBtn.image.sprite = Main.Mine.smaileImg3;
        else
            _smileBtn.image.sprite = Main.Mine.smaileImg2;

        _smileBtn.interactable = true;

        GameUiUpdate();
        TimerStop();
    }

    private void GameUiUpdate()
    {
        if (_leftBombText == null)
            return;

        _leftBombText.text = $"{Mathf.Clamp(Main.Mine.leftBomb, -99, 999)}";
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

    private void OnDisable()
    {
        CancelInvoke();

        Main.Mine.PressAction -= GameUiUpdate;
        Main.Mine.GameOverAction -= GameEnd;
        _exitBtn.onClick.RemoveListener(GameExit);
        _smileBtn.onClick.RemoveListener(GameSetting);
    }

    public void GameExit()
    {
        Main.Resource.Destroy(this.gameObject);
    }
}
